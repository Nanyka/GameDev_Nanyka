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

        // [Header("Outputs")]
        [HideInInspector] public List<Sprite> blueUnitSprites;
        [HideInInspector] public List<Sprite> redUnitSprites;
        [HideInInspector] public List<Sprite> remainAmountSprites;
        
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