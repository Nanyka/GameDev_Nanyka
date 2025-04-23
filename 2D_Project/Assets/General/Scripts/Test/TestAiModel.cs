using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class TestAiModel : MonoBehaviour
    {
        [SerializeField] private ModelAsset runtimeModel;
        [SerializeField] private GameObject circleObj;
        [SerializeField] private InventoryComp player1Inventory;
        [SerializeField] private InventoryComp player2Inventory;

        
        static BackendType backendType = BackendType.GPUCompute;
        private IWorker _worker;
        // private TensorFloat _inputTensor;
        // private TensorFloat _outputTensor;
        
        private void OnDisable()
        {
            // _inputTensor?.Dispose();
            // _outputTensor?.Dispose();
            _worker?.Dispose();
        }

        private async void Start()
        {
            await Init();
        }

        private async Task Init()
        {
            Model model = ModelLoader.Load(runtimeModel);
            // _worker = WorkerFactory.CreateWorker(backendType, model);
            
            // await CheckModel(model);

            await CheckEncoder(model);
        }

        private async Task CheckEncoder(Model model)
        {
            List<ICircleTrait> circlesOnBoard = new List<ICircleTrait>();
            int currentPlayerId = 2;
            GameObject obj = Instantiate(circleObj);

            if (obj.TryGetComponent(out ICircleTrait circle))
            {
                circlesOnBoard.Add(circle);
                circle.Init(new Vector3(0,0,0), 1, 1);
            }

            var stateInput = GameEncoder.EncodeBoardState(circlesOnBoard, currentPlayerId);
            var printState = "State: \n";
            for (int i = 0; i < stateInput.ToReadOnlyArray().Length; i++)
            {
                if (i % 3 == 0)
                    printState += $"\n{stateInput.ToReadOnlyArray()[i]} ";
                else
                    printState += $"{stateInput.ToReadOnlyArray()[i]} ";
            }
            Debug.Log(printState);
            
            // TODO: Generate input and check output from the model
            player1Inventory.ResetInventory();
            player2Inventory.ResetInventory();
            player2Inventory.Withdraw(1);
            
            var inventoryInput = GameEncoder.EncodeInventoryState(player1Inventory, 
                player2Inventory);

            var printInv = "Inventory: ";
            foreach (var item in inventoryInput.ToReadOnlyArray())
                printInv += $"{item}, ";
            Debug.Log(printInv);
            
            _worker = WorkerFactory.CreateWorker(backendType, model);
            Debug.Log($"Created engine with backend: {_worker.GetBackend()}");
        
            // Execute the Model
            string input1Name = model.inputs[0].name; 
            string input2Name = model.inputs[1].name; 
            var inputs = new Dictionary<string, Tensor>()
            {
                { input1Name, stateInput },
                { input2Name, inventoryInput }
            };
            _worker.Execute(inputs);
            
            string output1Name = model.outputs[0]; 
            string output2Name = model.outputs[1]; 
            
            TensorFloat output1 = _worker.PeekOutput() as TensorFloat; // Gets the first output by default
            TensorFloat output2 = _worker.PeekOutput(output2Name) as TensorFloat; // Get by name for clarity/safety
        
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
            var input1 = new TensorFloat(shape1, currentState);
            
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
            var input2 = new TensorFloat(shape2, currentInventory);
            
            // 3. Create an Inference Engine
            _worker = WorkerFactory.CreateWorker(backendType, model);
            Debug.Log($"Created engine with backend: {_worker.GetBackend()}");
        
            // 4. Execute the Model
            var inputs = new Dictionary<string, Tensor>()
            {
                { input1Name, input1 },
                { input2Name, input2 }
            };
            _worker.Execute(inputs);
        
            // 5. Read Output Tensors
            string output1Name = model.outputs[0]; 
            string output2Name = model.outputs[1]; 
            
            TensorFloat output1 = _worker.PeekOutput() as TensorFloat; // Gets the first output by default
            TensorFloat output2 = _worker.PeekOutput(output2Name) as TensorFloat; // Get by name for clarity/safety
        
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

        private static async Task ReadPolicyOutput(TensorFloat output1)
        {
            var returnAction = await output1.ReadbackRequestAsync();

            if (returnAction)
            {
                output1.MakeReadable();

                int output1Size = output1.shape[1]; // Size is the second dimension for (1, N) shape
                string printPrior = $"Output 1 Data (first 5 values): ";
                for (int i = 0; i < Mathf.Min(5, output1Size); i++)
                {
                    printPrior += $"[{i}] {output1[i]}; ";
                }

                Debug.Log(printPrior);
            }
        }
        
        private static async Task ReadValueOutput(TensorFloat output2)
        {
            var returnValue = await output2.ReadbackRequestAsync();

            if (returnValue)
            {
                output2.MakeReadable();
                Debug.Log($"Value output: {output2[0]}");
            }
        }
    }
}