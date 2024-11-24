using Core.ModifiableValue;
using UnityEngine;

namespace _Source.StatisticsSystem 
{
    [DisallowMultipleComponent]
    public class CharacterStats 
    {
        [Header("Character Stats")]
        [SerializeField] private ModifiableValue strength;
        [SerializeField] private ModifiableValue maxHealth;
        [SerializeField] private ModifiableValue speed;

        public CharacterStats(float basestrength, float basemaxHealth, float basespeed)
        {
            strength = new ModifiableValue(basestrength);
            maxHealth = new ModifiableValue(basemaxHealth);
            speed = new ModifiableValue(basespeed);
        }
    }
}
