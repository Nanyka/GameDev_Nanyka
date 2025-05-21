using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class UnitDropperVfx : MonoBehaviour
    {        
        [SerializeField] protected VoidChannel finishDropChannel;
        [SerializeField] protected IntChannel audioPlayIndex;
        
        [Header("Drop Settings")]
        public Transform unitTransform;      // your unit’s Transform
        public SpriteRenderer spriteRenderer; // The SpriteRenderer to fade
        public float dropHeight = 10f;       // how far above it starts
        public float dropDuration = 0.5f;    // how long the drop takes
        public float fadeDuration = 0.3f;    // Duration of the fade-in
        public Ease DropEase;

        [Header("Effects")]
        public ParticleSystem hitEffect;     // your landing‐burst particle

        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private Vector3 _originalPosition;
        private SpriteRenderer mainRenderer;
        
        private void Awake()
        {
            mainRenderer = GetComponent<SpriteRenderer>();
            _originalPosition = unitTransform.position;
        }

        public void PlayEffect(Sprite effectSprite, bool isBeatOpponent = false)
        {
            spriteRenderer.sprite = effectSprite;
            
            // 1. Cache start & target positions
            _targetPosition = unitTransform.position;
            _startPosition  = _targetPosition + Vector3.up * dropHeight;
            unitTransform.position = _startPosition;

            // 2. Build the sequence
            Sequence.Create()
                .Group(
                    Tween.Color(
                        target: spriteRenderer,
                        endValue: new Color(1f, 1f, 1f, 1f),
                        duration: fadeDuration
                    )
                )
                // Group is just for parallel tweens—here we only have the single Position tween
                .Group(
                    Tween.Position(
                        unitTransform,
                        endValue: _targetPosition,
                        duration: dropDuration,
                        ease: DropEase
                    )
                )
                // Chain a callback so it fires right after the drop finishes
                .ChainCallback(() =>
                {
                    if (hitEffect != null)
                    {
                        hitEffect.transform.position = _targetPosition;
                        hitEffect.Play();
                        audioPlayIndex.ExecuteChannel(isBeatOpponent ? 1 : 0);
                    }
                    
                    spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
                    mainRenderer.sprite = effectSprite;
                    if (mainRenderer != null) mainRenderer.enabled = true;
                    unitTransform.position = _originalPosition;
                    finishDropChannel.ExecuteChannel();
                })
                // (Optional) you could ChainDelay or more tweens here
                ;
        }
    }
}