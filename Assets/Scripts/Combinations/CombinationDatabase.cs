using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace Database 
{
    public class CombinationDatabase : MonoBehaviour
    {

        [SerializeField] private List<Recipe> recipes = new List<Recipe>();
        [SerializeField] private AnimalSO _human;
        public static List<Recipe> StaticRecipes;

        private static AnimalSO _staticHuman;

        public static Action<AnimalSO> OnCorrectCombination;
        public static Action OnFinalCombination;

        private void Awake()
        {
            StaticRecipes = null;
            StaticRecipes = new List<Recipe>(recipes);

            _staticHuman = null;
            _staticHuman = _human;
        }

        public static AnimalSO CheckIfRecipieValid(AnimalSO animalInput1, AnimalSO animalInput2)
        {
            AnimalSO animalChoice = CheckAnimalRecipe(animalInput1, animalInput2);

            return animalChoice;
        }

        public static AnimalSO CheckAnimalRecipe(AnimalSO itemInput1, AnimalSO itemInput2)
        {
            AnimalSO animalChoice = null;

            // This line will break things if any recipes are made up of two of the same animal.
            if (AreInputsIdentical(itemInput1, itemInput2)) return animalChoice;

            foreach (Recipe recipe in StaticRecipes)
            {
                if (!AreInputsIdentical(recipe.inputs[0], itemInput1) && !AreInputsIdentical(recipe.inputs[1], itemInput1)) continue;
                if (!AreInputsIdentical(recipe.inputs[0], itemInput2) && !AreInputsIdentical(recipe.inputs[1], itemInput2)) continue;

                Debug.Log("Combined into " + recipe.output.ToString());
                // instantiate a completed recipe
                animalChoice = recipe.output;

                OnCorrectCombination?.Invoke(animalChoice);

                if (animalChoice == _staticHuman)
                {
                    OnFinalCombination?.Invoke();
                }
            }

            return animalChoice;
        }

        public static bool AreInputsIdentical(AnimalSO animal1, AnimalSO animal2)
        {
            if (animal1 == animal2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void GetValidRecipe(AnimalSO input, out AnimalSO output1, out AnimalSO output2)
        {
            AnimalSO tempOutput1 = null;
            AnimalSO tempOutput2 = null;
            foreach (Recipe recipe in StaticRecipes)
            {
                if (recipe.output == input)
                {
                    tempOutput1 = recipe.inputs[0];
                    tempOutput2 = recipe.inputs[1];
                }
            }

            output1 = tempOutput1;
            output2 = tempOutput2;
        }
    }



    [System.Serializable]
    public class Recipe
    {
        public string nameString;
        public AnimalSO[] inputs = new AnimalSO[2];
        public AnimalSO output;
    }
}
