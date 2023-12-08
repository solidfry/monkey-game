using System;
using System.Linq;
using Database;
using DG.Tweening;
using Enums;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Combinations
{
    public class DragDrop : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _animalRb;
        private Vector2 _offset;

        [SerializeField] private GameObject animalTokenPrefab;
        // [SerializeField] private CombinationDatabase combinationDB;
        
        [SerializeField] private Ease animationDragEase = Ease.InOutSine;
        [SerializeField] private Ease animationDropEase = Ease.OutBounce;

        [SerializeField]
        float scaleOnDrag = 1.2f;
        [SerializeField]
        float scaleOnDrop = 1f;
        [SerializeField]
        float scaleDuration = 0.1f;

        [Header("Drop Punch")] 
        [SerializeField] private float punchDuration = .2f;
        [SerializeField] private int punchVibrato = 2;
        [SerializeField] private float punchElasticity = 4f;
        [SerializeField] Vector2 punch = new Vector2(.1f, .3f);

        readonly AnimalToken[] _animals = new AnimalToken[2];
        readonly Collider2D[] _colliders = new Collider2D[2];

        private Tween DragScale => transform.DOScale(scaleOnDrag, scaleDuration);
        private Tween DropScale => transform.DOScale(scaleOnDrop, scaleDuration);
        private Tween DropPunch => transform.DOPunchScale(punch, punchDuration, punchVibrato, punchElasticity);
        private TweenParams DragParams => new TweenParams().SetEase(animationDragEase).SetAutoKill(false);
        private TweenParams DropParams => new TweenParams().SetEase(animationDropEase).SetAutoKill(false);

        private Sequence Drag => 
            DOTween.Sequence()
                .Append(DragScale)
                .SetAs(DragParams);
        private Sequence Drop => 
            DOTween.Sequence()
                .Append(DropScale).AppendCallback(() =>
                {
                    DropPunch.Rewind(); 
                    DropPunch.Play(); 
                })
                .SetAs(DropParams); 

        void Start()
        {
            _animalRb = GetComponent<Rigidbody2D>();
            _animalRb.bodyType = RigidbodyType2D.Kinematic;
            this.GetComponents<SpriteRenderer>().ToList().ForEach(sr => sr.DOFade( 1, .5f));
            transform.DOScale(1, .5f).From(0).SetEase(Ease.OutBounce).SetAutoKill(true);
        }

        void OnMouseDown()
        {
            if (Camera.main != null)
                _offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(Drop == null || Drag == null) return;
        
            // Drop.Rewind();
            Drag.Play();
        }

        void OnMouseDrag()
        {
            if (Camera.main != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _animalRb.MovePosition(mousePosition + _offset);
            }

            transform.localScale = Vector3.one * scaleOnDrag;
        }

        void OnMouseUp()
        {
            // Check for overlap after releasing the object
            CheckOverlap();
        
            if(Drop == null || Drag == null) return;
        
            Drop.Play();
            Drag.Rewind();
        }

        void CheckOverlap()
        {
            var t = transform;
            var size = Physics2D.OverlapBoxNonAlloc(t.position, t.localScale, 0f, _colliders);
            bool isOverlapping = true;

            if(size <= 1) return;
            
            foreach (Collider2D col in _colliders)
            {
                if (!col.CompareTag("Animal"))
                {
                    isOverlapping = false;
                }
            }

            if (!isOverlapping || size != 2) return;
        
            for (int i = 0; i < _animals.Length; i++)
            {
                if (!_colliders[i].TryGetComponent<AnimalToken>(out var token))
                {
                    i--;
                    continue;
                }
                _animals[i] = token;
            }

            var newAnimal = CombinationDatabase.CheckAnimalRecipe(_animals[0].AnimalData, _animals[1].AnimalData);
            if (newAnimal != null)
            {
                GameObject newToken = Instantiate(animalTokenPrefab, transform.position, Quaternion.identity);
                newToken.GetComponent<AnimalToken>().Initialise(newAnimal);
                foreach (var animal in _animals)
                {
                    GameObject animalGo = animal.gameObject;
                    animalGo.GetComponent<SpriteRenderer>()
                        .DOFade(0, .25f)
                        .SetEase(Ease.Linear);
                    
                    animalGo.transform.DOScale(0.1f, .25f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => animalGo.SetActive(false));
                    
                    Destroy(animalGo, .5f);
                }
            
                TokenParticleManager.OnTokenParticleSpawn?.Invoke(transform.position, ParticleType.Merge);
            }
        
            Array.Clear(_animals, 0, _animals.Length);
        }

        public void ExternalDrag()
        {
            if (Camera.main != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _animalRb.MovePosition(mousePosition + _offset);
            }

            transform.localScale = Vector3.one * scaleOnDrag;
        }
    }
}
