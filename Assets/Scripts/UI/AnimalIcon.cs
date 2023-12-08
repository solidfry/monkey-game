using Combinations;
using Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimalIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public AnimalSO AnimalData;
    [SerializeField] Image animalSpriteRenderer;
    [SerializeField] TMP_Text animalNameText;
    [SerializeField] private GameObject animalTokenPrefab;

    private GameObject currentToken = null;

    private void Start()
    {
        if (AnimalData == null) return;
        Debug.Log(AnimalData.GetNameString() + " initialised");
        animalSpriteRenderer.sprite = AnimalData.GetSprite();
        animalNameText.text = AnimalData.GetNameString();
    }

    public void Initialise(AnimalSO animalData)
    {
        if (animalData == null) return;
        Debug.Log(animalData.GetNameString() + " initialised");
        AnimalData = animalData;
        animalSpriteRenderer.sprite = animalData.GetSprite();
        animalNameText.text = animalData.GetNameString();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        GameObject newToken = Instantiate(animalTokenPrefab, position, Quaternion.identity);
        newToken.GetComponent<AnimalToken>().Initialise(AnimalData);
        currentToken = newToken;
        Debug.Log(AnimalData.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentToken.GetComponent<DragDrop>().ExternalDrag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentToken = null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (ValidateValues()) return;

        Initialise(AnimalData);
    }

    private bool ValidateValues() => animalSpriteRenderer.sprite == AnimalData.GetSprite() && animalNameText.text == AnimalData.GetNameString();
#endif

    public int GetItemTier() => AnimalData.GetItemTier();
}
