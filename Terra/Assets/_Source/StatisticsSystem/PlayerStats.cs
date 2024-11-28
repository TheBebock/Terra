using UnityEngine;
using Core.ModifiableValue;
namespace _Source.StatisticsSystem
{
    [DisallowMultipleComponent]
    public class PlayerStats : CharacterStats
    {
        [Header("Player Stats")]
        [SerializeField] private ModifiableValue luck;

        public PlayerStats(float basestrength, float basemaxHealth, float basespeed, float baseLuck) 
            : base(basestrength, basemaxHealth, basespeed)
        {
            luck = new ModifiableValue(baseLuck);
        }
        public float Luck => luck.Value;
        public void AddLuckModifier(ValueModifier modifier)
        {
            luck.AddStatModifier(modifier);
        }

        // Usuwanie modyfikatora z luck
        public bool RemoveLuckModifier(ValueModifier modifier)
        {
            return luck.RemoveStatModifier(modifier);
        }
    }
}