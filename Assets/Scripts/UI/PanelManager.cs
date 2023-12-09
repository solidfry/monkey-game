using Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{

    [SerializeField] private List<AnimalIcon> animalIcons;

    public void AddNewIcon(AnimalSO animalSO)
    {
        foreach (var icon in animalIcons)
        {
            if (icon.AnimalData != animalSO) continue;
            print("add icon " + icon);
            icon.gameObject.SetActive(true);
        }
    }

    private void OnEnable() => CombinationDatabase.OnCorrectCombination += AddNewIcon;
    private void OnDisable() => CombinationDatabase.OnCorrectCombination -= AddNewIcon;
}
