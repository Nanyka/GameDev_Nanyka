using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaZeroAlgorithm;
using Unity.Sentis;
using UnityEngine;

namespace TheAiAlchemist
{
    public class TestAiModel : MonoBehaviour
    {
        [SerializeField] private ModelAsset runtimeModel;

        
        static BackendType backendType = BackendType.GPUCompute;
        private Worker _worker;
        
        private void OnDisable()
        {
            _worker?.Dispose();
        }

        private async void Start()
        {
            // SaveSentisModel();
            await Init();
        }

        private async Task Init()
        {
            Model model = ModelLoader.Load(runtimeModel);
            // _worker = WorkerFactory.CreateWorker(backendType, model);
            
            // await CheckModel(model);
            await CheckEncoder(model);

            CheckMCTSAlgorithm();
        }

        private async Task CheckMCTSAlgorithm()
        {
            var botAgent = new AlphaZeroAgent(runtimeModel,52, printOut:true);
            var gameState = GameSetup.SetupNewGame();
            gameState = gameState.ApplyMove(new Move(new Point(1,2,1)));
            gameState = gameState.ApplyMove(new Move(new Point(2,2,3)));
            gameState = gameState.ApplyMove(new Move(new Point(1,3,2)));

            var selectedMove = await botAgent.SelectMove(gameState);
            print($"Selected move: {selectedMove}");
        }

        private async Task CheckEncoder(Model model)
        {
            var encoder = new Encoder();
            var gameState = GameSetup.SetupNewGame();
            gameState = gameState.ApplyMove(new Move(new Point(1,2,1)));
            gameState = gameState.ApplyMove(new Move(new Point(2,2,3)));
            gameState = gameState.ApplyMove(new Move(new Point(1,3,2)));
            Debug.Log(gameState.ToString());

            var (stateInput, inventoryInput) = encoder.Encode(gameState);

            _worker = new Worker(model, backendType);
            // Debug.Log($"Created engine with backend: {_worker.GetBackend()}");
        
            // Execute the Model
            string input1Name = model.inputs[0].name; 
            string input2Name = model.inputs[1].name; 
            // var inputs = new Dictionary<string, Tensor>()
            // {
            //     { input1Name, stateInput },
            //     { input2Name, inventoryInput }
            // };
            
            _worker.SetInput(input1Name,stateInput);
            _worker.SetInput(input2Name,inventoryInput);
            
            _worker.Schedule();
            
            string output1Name = model.outputs[0].name; 
            string output2Name = model.outputs[1].name;
            
            Tensor<float> output1 = _worker.PeekOutput() as Tensor<float>; // Gets the first output by default
            Tensor<float> output2 = _worker.PeekOutput(output2Name) as Tensor<float>; // Get by name for clarity/safety
        
            Debug.Log($"\nOutput 1 Name: {output1Name}, Shape: {output1.shape}");
            
            await ReadPolicyOutput(output1);

            await ReadValueOutput(output2);

            // --- Cleanup ---
            // 6. Dispose of Tensors and the Engine
            stateInput.Dispose();
            inventoryInput.Dispose();
            output1.Dispose();
            output2.Dispose();
            _worker.Dispose();
            Debug.Log("\nInference complete and resources disposed.");
        }

        private async Task CheckModel(Model model)
        {
            print($"{model.inputs[0].name}: {model.inputs[0].shape}");
            print($"{model.inputs[1].name}: {model.inputs[1].shape}");
            print($"{model.outputs[0]}");
            print($"{model.outputs[1]}");
            
            // 1. Create Input Tensors
            string input1Name = model.inputs[0].name; 
            string input2Name = model.inputs[1].name; 
            
            int batchSize = 1;
            TensorShape shape1 = new TensorShape(batchSize, 7, 3, 3);
            TensorShape shape2 = new TensorShape(batchSize, 6);
        
            // 2. Create dummy input data for demonstration
            // For input 1 (1, 7, 3, 3)
            var currentState = new float[7*3*3];
            for (int b = 0; b < batchSize; b++)
            {
                for (int c = 0; c < 7; c++)
                {
                    for (int h = 0; h < 3; h++)
                    {
                        for (int w = 0; w < 3; w++)
                        {
                            // Example: assign sequential value
                            currentState[b*c*h + w] = (float)((b * 7 * 3 * 3) + (c * 3 * 3) + (h * 3) + w);
                        }
                    }
                }
            }
            var input1 = new Tensor<float>(shape1, currentState);
            
            // For input 2 (1, 6)
            var currentInventory = new float[6];
            for (int b = 0; b < batchSize; b++)
            {
                for (int i = 0; i < 6; i++)
                {
                    // Example: assign sequential value
                    currentInventory[b + i] = (float)((b * 6) + i);
                }
            }
            var input2 = new Tensor<float>(shape2, currentInventory);
            
            // 3. Create an Inference Engine
            _worker = new Worker(model, backendType);
            // Debug.Log($"Created engine with backend: {_worker.GetBackend()}");
        
            // 4. Execute the Model
            // var inputs = new Dictionary<string, Tensor>()
            // {
            //     { input1Name, input1 },
            //     { input2Name, input2 }
            // };
            _worker.SetInput(input1Name,input1);
            _worker.SetInput(input2Name,input2);
            _worker.Schedule();
        
            // 5. Read Output Tensors
            string output1Name = model.outputs[0].name; 
            string output2Name = model.outputs[1].name; 
            
            Tensor<float> output1 = _worker.PeekOutput() as Tensor<float>; // Gets the first output by default
            Tensor<float> output2 = _worker.PeekOutput(output2Name) as Tensor<float>; // Get by name for clarity/safety
        
            Debug.Log($"\nOutput 1 Name: {output1Name}, Shape: {output1.shape}");
            
            await ReadPolicyOutput(output1);

            await ReadValueOutput(output2);

            // --- Cleanup ---
            // 6. Dispose of Tensors and the Engine
            input1.Dispose();
            input2.Dispose();
            output1.Dispose();
            output2.Dispose();
            _worker.Dispose();
            Debug.Log("\nInference complete and resources disposed.");
        }

        private static async Task ReadPolicyOutput(Tensor<float> output1)
        {
            var returnAction = await output1.ReadbackAndCloneAsync();

            if (returnAction != null)
            {
                // output1.MakeReadable();

                int output1Size = returnAction.shape[1]; // Size is the second dimension for (1, N) shape
                string printPrior = $"Output 1 Data (first 5 values): ";
                for (int i = 0; i < Mathf.Min(5, output1Size); i++)
                {
                    printPrior += $"[{i}] {returnAction[i]}; ";
                }

                Debug.Log(printPrior);
            }
        }
        
        private async Task ReadValueOutput(Tensor<float> output2)
        {
            var returnValue = await output2.ReadbackAndCloneAsync();

            if (returnValue != null)
            {
                // output2.MakeReadable();
                Debug.Log($"Value output: {returnValue[0]}");
            }
        }

        private void SaveSentisModel()
        {
            Model model = ModelLoader.Load(runtimeModel);
            ModelWriter.Save("modelSentis.sentis",model);
        }
    }
}