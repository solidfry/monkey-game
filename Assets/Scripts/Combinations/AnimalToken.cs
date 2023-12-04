using Database;
using TMPro;
using UnityEngine;

public class AnimalToken : MonoBehaviour
{
    public AnimalSO AnimalData;
    [SerializeField] SpriteRenderer animalSpriteRenderer;
    [SerializeField] TMP_Text animalNameText;
    
    void Start() => Initialise(AnimalData);
    
    public void Initialise(AnimalSO animalData)
    {
        if(animalData == null) return;
        this.AnimalData = animalData;
        animalSpriteRenderer.sprite = animalData.GetSprite();
        animalNameText.text = animalData.GetNameString();
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(ValidateValues()) return;
        
        Initialise(AnimalData);
    }

    private bool ValidateValues() => animalSpriteRenderer.sprite == AnimalData.GetSprite() && animalNameText.text == AnimalData.GetNameString();
#endif
    
    public int GetItemTier() => AnimalData.GetItemTier();
}
