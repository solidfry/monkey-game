using Database;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UnityEngine;

public class CheckCombinationTest : MonoBehaviour
{
    private int clickAmount = 0;
    private AnimalSO input1;

    public void ButtonPressed(AnimalSO animalSO)
    {
        clickAmount++;
        print(animalSO.GetNameString());

        if (clickAmount >= 2)
        {
            clickAmount = 0;
            AnimalSO output = CombinationDatabase.CheckAnimalRecipe(input1, animalSO);
            return;
        }

        input1 = animalSO;
 
    }
}
