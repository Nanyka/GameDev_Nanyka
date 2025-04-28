using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "GeneralAssetLoader", menuName = "TheAiAlchemist/BackEnd/GeneralAssetLoader")]
    public class GeneralAssetLoader : ScriptableObject
    {
        [Header("Inputs")]
        public string[] blueUnitAddress;
        public string[] redUnitAddress;
        public string[] remainAmountAddress;

        [Header("Outputs")]
        public List<Sprite> blueUnitSprites;
        public List<Sprite> redUnitSprites;
        public List<Sprite> remainAmountSprites;
        
        public void ResetBlueSprites(List<Sprite> sprites)
        {
            blueUnitSprites = sprites;
        }
        
        public void ResetRedSprites(List<Sprite> sprites)
        {
            redUnitSprites = sprites;
        }
        
        public void ResetRemainSprites(List<Sprite> sprites)
        {
            remainAmountSprites = sprites;
        }
    }
}