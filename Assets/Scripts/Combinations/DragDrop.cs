using Database;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class DragDrop : MonoBehaviour
{
    private Rigidbody2D animalRB;
    private Vector2 offset;

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
    
    private TweenParams dragParams => new TweenParams().SetEase(animationDragEase);
    private TweenParams dropParams => new TweenParams().SetEase(animationDropEase);

    private Sequence Drag => DOTween.Sequence()
        .Append(transform.DOScale(scaleOnDrag, scaleDuration).SetAs(dragParams).SetAutoKill(false));
    private Sequence Drop => DOTween.Sequence()
        .Append(transform.DOScale(scaleOnDrop, scaleDuration)
            .SetAs(dropParams))
        .AppendCallback(() =>
        {
            transform.DOPunchScale(punch, punchDuration, punchVibrato, punchElasticity);
        }).SetAutoKill(false); 

    void Start()
    {
        animalRB = GetComponent<Rigidbody2D>();
        animalRB.bodyType = RigidbodyType2D.Kinematic;
       
    }

    void OnMouseDown()
    {
        offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        Drop.Play();
        Drag.Rewind();
    }
    
    void CheckOverlap()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        bool isOverlapping = false;

        foreach (Collider2D collider in colliders)
        {
            if (collider == GetComponent<Collider2D>())
                continue;

            if (collider.CompareTag("Animal"))
            {
                isOverlapping = true;
                Debug.Log("Join the animals together");
            }
        }

        if (!isOverlapping) return;

        AnimalSO[] animals = new AnimalSO[2];
        for (int i = 0; i < animals.Length; i++)
        {
            if (!colliders[i].TryGetComponent<AnimalToken>(out var token))
            {
                i--;
                continue;
            }
            animals[i] = token.AnimalData;
        }

        CombinationDatabase.CheckAnimalRecipe(animals[0], animals[1]);
    }
}
