using System;
using Database;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class DragDrop : MonoBehaviour
{
    private Rigidbody2D animalRB;
    private Vector2 offset;

    [SerializeField] private GameObject animalTokenPrefab;
    [SerializeField] private CombinationDatabase combinationDB;

   

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
    
    AnimalToken[] animals = new AnimalToken[2];

    private Tween dragScale => transform.DOScale(scaleOnDrag, scaleDuration);
    private Tween dropScale => transform.DOScale(scaleOnDrop, scaleDuration);
    private Tween dropPunch => transform.DOPunchScale(punch, punchDuration, punchVibrato, punchElasticity);
    private TweenParams dragParams => new TweenParams().SetEase(animationDragEase).SetAutoKill(false).SetRecyclable(false);
    private TweenParams dropParams => new TweenParams().SetEase(animationDropEase).SetAutoKill(false).SetRecyclable(false);

    private Sequence Drag => 
        DOTween.Sequence()
        .Append(dragScale)
        .SetAs(dragParams);
    private Sequence Drop => 
        DOTween.Sequence()
        .Append(dropScale).AppendCallback(()=>dropPunch.Play())
        .SetAs(dropParams); 

    void Start()
    {
        animalRB = GetComponent<Rigidbody2D>();
        animalRB.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnMouseDown()
    {
        offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Drop == null || Drag == null) return;
        
        Drop.Rewind();
        Drag.Play();
        
    }

    void OnMouseDrag()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        animalRB.MovePosition(mousePosition + offset);
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
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        bool isOverlapping = true;

        foreach (Collider2D collider in colliders)
        {
            if (!collider.CompareTag("Animal"))
            {
                isOverlapping = false;
            }
        }

        if (!isOverlapping || colliders.Length != 2) return;
        
        for (int i = 0; i < animals.Length; i++)
        {
            if (!colliders[i].TryGetComponent<AnimalToken>(out var token))
            {
                i--;
                continue;
            }
            animals[i] = token;
        }

        var newAnimal = CombinationDatabase.CheckAnimalRecipe(animals[0].AnimalData, animals[1].AnimalData);
        if (newAnimal != null)
        {
            GameObject newToken = Instantiate(animalTokenPrefab);
            newToken.GetComponent<AnimalToken>().Initialise(newAnimal);
            foreach (var animal in animals)
            {
                GameObject animalGo = animal.gameObject;
                animalGo.SetActive(false);
                Destroy(animalGo.gameObject,1);
            }
            
        }
        
        Array.Clear(animals, 0, animals.Length);
    }
}
