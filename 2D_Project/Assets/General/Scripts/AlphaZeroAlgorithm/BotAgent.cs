using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Sentis;
using UnityEngine;

namespace AlphaZeroAlgorithm
{
    public class BotAgent : IAgent
    {
        private Model _runtimeModel;
        private IWorker _worker;
        private Encoder _encoder;

        public BotAgent(ModelAsset model)
        {
            _encoder = new Encoder();
            _runtimeModel = ModelLoader.Load(model);
            _worker = WorkerFactory.CreateWorker(BackendType.CPU, _runtimeModel);
            
        }

        public async Task<Move> SelectMove(GameState gameState)
        {
            string input1Name = _runtimeModel.inputs[0].name; 
            string input2Name = _runtimeModel.inputs[1].name;
            var encodedInputs = _encoder.Encode(gameState);
            
            var inputs = new Dictionary<string, Tensor>()
            {
                { input1Name, encodedInputs.boardTensor },
                { input2Name, encodedInputs.inventoryTensor }
            };
            _worker.Execute(inputs);
            
            string output2Name = _runtimeModel.outputs[1]; 
            
            TensorFloat output1 = _worker.PeekOutput() as TensorFloat;
            TensorFloat output2 = _worker.PeekOutput(output2Name) as TensorFloat;

            var nextMove = Move.Resign;
            if (output1 != null)
            {
                var maxMoveIndex = ArgMax(output1.ToReadOnlyArray());
                nextMove = _encoder.DecodeMoveIndex(maxMoveIndex);
            }
            
            encodedInputs.boardTensor.Dispose();
            encodedInputs.inventoryTensor.Dispose();
            if (output1 != null) output1.Dispose();
            if (output2 != null) output2.Dispose();
            _worker.Dispose();

            return nextMove;
        }

        public Move GetMoveFromInput(string inputString)
        {
            Debug.Log("The bot can not use Input Field");
            return Move.Resign;
        }

        public async Task TestBot(GameState gameState)
        {
            string input1Name = _runtimeModel.inputs[0].name; 
            string input2Name = _runtimeModel.inputs[1].name;
            var encodedInputs = _encoder.Encode(gameState);
            
            var inputs = new Dictionary<string, Tensor>()
            {
                { input1Name, encodedInputs.boardTensor },
                { input2Name, encodedInputs.inventoryTensor }
            };
            _worker.Execute(inputs);
            
            string output2Name = _runtimeModel.outputs[1]; 
            
            TensorFloat output1 = _worker.PeekOutput() as TensorFloat; // Gets the first output by default
            TensorFloat output2 = _worker.PeekOutput(output2Name) as TensorFloat; // Get by name for clarity/safety
            
            await ReadPolicyOutput(output1);
            await ReadValueOutput(output2);

            var maxMoveIndex = ArgMax(output1.ToReadOnlyArray());
            var nextMove = _encoder.DecodeMoveIndex(maxMoveIndex);
            Debug.Log($"Max prior move: {nextMove}");

            // --- Cleanup ---
            // 6. Dispose of Tensors and the Engine
            encodedInputs.boardTensor.Dispose();
            encodedInputs.inventoryTensor.Dispose();
            if (output1 != null) output1.Dispose();
            if (output2 != null) output2.Dispose();
            _worker.Dispose();
            Debug.Log("Inference complete and resources disposed.");
        }
        
        private async Task ReadPolicyOutput(TensorFloat output1)
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
        
        private async Task ReadValueOutput(TensorFloat output2)
        {
            var returnValue = await output2.ReadbackRequestAsync();

            if (returnValue)
            {
                output2.MakeReadable();
                Debug.Log($"Value output: {output2[0]}");
            }
        }
        
        public static int ArgMax(float[] array)
        {
            // Check for null or empty array
            if (array == null || array.Length == 0)
            {
                return -1; // Return -1 to indicate no valid index found
            }

            int maxIndex = 0; // Start with the first element's index
            float maxValue = array[0]; // Start with the first element's value

            // Iterate through the array starting from the second element
            for (int i = 1; i < array.Length; i++)
            {
                // If the current element's value is greater than the current max value
                if (array[i] > maxValue)
                {
                    maxValue = array[i]; // Update the max value
                    maxIndex = i;       // Update the index of the max value
                }
            }

            return maxIndex; // Return the index of the maximum value
        }
    }
}