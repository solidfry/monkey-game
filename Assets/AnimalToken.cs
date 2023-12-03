using Database;
using TMPro;
using UnityEngine;

public class AnimalToken : MonoBehaviour
{
    [SerializeField] AnimalSO animalData;
    [SerializeField] SpriteRenderer animalSpriteRenderer;
    [SerializeField] TMP_Text animalNameText;
    
    void Start() => Initialise(animalData);
    
    public void Initialise(AnimalSO animalData)
    {
        if(animalData == null) return;
        this.animalData = animalData;
        animalSpriteRenderer.sprite = animalData.GetSprite();
        animalNameText.text = animalData.GetNameString();
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(ValidateValues()) return;
        
        Initialise(animalData);
    }

    private bool ValidateValues() => animalSpriteRenderer.sprite == animalData.GetSprite() && animalNameText.text == animalData.GetNameString();
#endif
    
    public int GetItemTier() => animalData.GetItemTier();
}
