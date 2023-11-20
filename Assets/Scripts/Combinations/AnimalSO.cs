using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    [CreateAssetMenu()]
    public class AnimalSO : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private string nameString;
        [SerializeField] private int animalTier;

        public int GetItemTier()
        {
            return animalTier;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public string GetNameString()
        {
            return nameString;
        }
    }
}

