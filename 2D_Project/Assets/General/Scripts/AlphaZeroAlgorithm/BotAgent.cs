using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Sentis;
using UnityEngine;

namespace AlphaZeroAlgorithm
{
    public class BotAgent : IAgent
    {
        private readonly Model _runtimeModel;
        private readonly Worker _worker;
        private readonly Encoder _encoder;

        public BotAgent(ModelAsset model)
        {
            _encoder = new Encoder();
            _runtimeModel = ModelLoader.Load(model);
            _worker = new Worker(_runtimeModel, BackendType.CPU);

        }

        public Task<Move> SelectMove(GameState gameState)
        {
            var input1Name = _runtimeModel.inputs[0].name; 
            var input2Name = _runtimeModel.inputs[1].name;
            var encodedInputs = _encoder.Encode(gameState);
            
            // var inputs = new Dictionary<string, Tensor>()
            // {
            //     { input1Name, encodedInputs.boardTensor },
            //     { input2Name, encodedInputs.inventoryTensor }
            // };
            Tensor[] inputs = { encodedInputs.boardTensor, encodedInputs.inventoryTensor };
            _worker.Schedule(inputs);
            
            var output2Name = _runtimeModel.outputs[1].name; 
            
            var output1 = _worker.PeekOutput() as Tensor<float>;
            var output2 = _worker.PeekOutput(output2Name) as Tensor<float>;

            var nextMove = Move.Resign;
            if (output1 != null)
            {
                var maxMoveIndex = ArgMax(output1.DownloadToArray());
                nextMove = _encoder.DecodeMoveIndex(maxMoveIndex);
            }
            
            encodedInputs.boardTensor.Dispose();
            encodedInputs.inventoryTensor.Dispose();
            output1?.Dispose();
            output2?.Dispose();
            _worker.Dispose();

            return Task.FromResult(nextMove);
        }

        public Move GetMoveFromInput(string inputString)
        {
            Debug.Log("The bot can not use Input Field");
            return Move.Resign;
        }

        public async Task TestBot(GameState gameState)
        {
            var input1Name = _runtimeModel.inputs[0].name; 
            var input2Name = _runtimeModel.inputs[1].name;
            var encodedInputs = _encoder.Encode(gameState);
            
            // var inputs = new Dictionary<string, Tensor>()
            // {
            //     { input1Name, encodedInputs.boardTensor },
            //     { input2Name, encodedInputs.inventoryTensor }
            // };
            Tensor[] inputs = { encodedInputs.boardTensor, encodedInputs.inventoryTensor };

            _worker.Schedule(inputs);
            
            var output2Name = _runtimeModel.outputs[1].name; 
            
            var output1 = _worker.PeekOutput() as Tensor<float>; // Gets the first output by default
            var output2 = _worker.PeekOutput(output2Name) as Tensor<float>; // Get by name for clarity/safety
            
            await ReadPolicyOutput(output1);
            await ReadValueOutput(output2);

            if (output1 != null)
            {
                var maxMoveIndex = ArgMax(output1.DownloadToArray());
                var nextMove = _encoder.DecodeMoveIndex(maxMoveIndex);
                Debug.Log($"Max prior move: {nextMove}");
            }

            // --- Cleanup ---
            // 6. Dispose of Tensors and the Engine
            encodedInputs.boardTensor.Dispose();
            encodedInputs.inventoryTensor.Dispose();
            output1?.Dispose();
            output2?.Dispose();
            _worker.Dispose();
            Debug.Log("Inference complete and resources disposed.");
        }
        
        private static async Task ReadPolicyOutput(Tensor<float> output1)
        {
            var returnAction = await output1.ReadbackAndCloneAsync();

            if (returnAction != null)
            {
                // output1.MakeReadable();

                int output1Size = output1.shape[1]; // Size is the second dimension for (1, N) shape
                string printPrior = $"Output 1 Data (first 5 values): ";
                for (int i = 0; i < Mathf.Min(5, output1Size); i++)
                {
                    printPrior += $"[{i}] {output1[i]}; ";
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
                Debug.Log($"Value output: {output2[0]}");
            }
        }

        private static int ArgMax(IReadOnlyList<float> array)
        {
            // Check for null or empty array
            if (array == null || array.Count == 0)
            {
                return -1; // Return -1 to indicate no valid index found
            }

            var maxIndex = 0; // Start with the first element's index
            var maxValue = array[0]; // Start with the first element's value

            // Iterate through the array starting from the second element
            for (int i = 1; i < array.Count; i++)
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