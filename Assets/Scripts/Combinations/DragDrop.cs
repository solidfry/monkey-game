using Database;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private Rigidbody2D animalRB;
    private Vector2 offset;

    [SerializeField] private CombinationDatabase combinationDB;

    void Start()
    {
        animalRB = GetComponent<Rigidbody2D>();
        animalRB.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnMouseDown()
    {
        offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
    }

    void CheckOverlap()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider == GetComponent<Collider2D>())
                continue;

            if (collider.CompareTag("Animal"))
            {
                Debug.Log("Join the animals together");
            }
        }
    }
}
