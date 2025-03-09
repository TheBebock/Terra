using System.Collections.Generic;
using Core.Generics;
using Core.ModifiableValue;
using StatisticsSystem;
using StatisticsSystem.Definitions;
using UnityEngine;

namespace _Source.Player
{
    public class PlayerStatsManager : MonoBehaviourSingleton<PlayerStatsManager>
    {
        [SerializeField] private  PlayerStatsDefinition playerStatsData;
        [SerializeField] private  PlayerStats playerStats;
        
        public PlayerStats PlayerStats => playerStats;
        protected override void Awake()
        {
            base.Awake();
            playerStats = new PlayerStats(playerStatsData);
        }
        
        //TODO: Maybe in future improve this
        /*
        public void Add/Remove Modifiers(List<T> modifiers)
        {

        }

        public void AddModifier<T>(T modifier)
            where T : ValueModifier
        {
            playerStats.Add(modifier);
        }

        public void RemoveModifier<T>(T modifier)
            where T : ValueModifier
        {
            playerStats.Remove(modifier);
        }
        */

        public void AddStrength(ValueModifier modifier)
        {
            playerStats.AddStrengthModifier(modifier);
        }
        public void AddStrength(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                playerStats.AddStrengthModifier(modifiers[i]);
            }
        }
        
        public void RemoveStrength(ValueModifier modifier)
        {
            playerStats.RemoveStrengthModifier(modifier);
        }
        public void RemoveStrength(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                RemoveStrength(modifiers[i]);
            }
        }

        public void AddMaxHealth(ValueModifier modifier)
        {
            playerStats.AddMaxHealthModifier(modifier);
        }
        public void AddMaxHealth(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                AddMaxHealth(modifiers[i]);
            }
        }

        public void RemoveMaxHealth(ValueModifier modifier)
        {
            playerStats.RemoveMaxHealthModifier(modifier);
        }
        public void RemoveMaxHealth(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            { 
                RemoveMaxHealth(modifiers[i]);
            }
        }



        public void AddSpeed(ValueModifier modifier)
        {
            playerStats.AddSpeedModifier(modifier);
        }
        public void AddSpeed(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                AddSpeed(modifiers[i]);
            }
        }

        public void RemoveSpeed(ValueModifier modifier)
        {
            playerStats.RemoveSpeedModifier(modifier);
        }
        public void RemoveSpeed(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                RemoveSpeed(modifiers[i]);
            }
        }
        
        public void AddLuck(ValueModifier modifier)
        {
            playerStats.AddLuckModifier(modifier);
        }
        public void AddLuck(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                AddLuck(modifiers[i]);
            }
        }

        public void RemoveLuck(ValueModifier modifier)
        {
            playerStats.RemoveLuckModifier(modifier);
        }
        public void RemoveLuck(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                RemoveLuck(modifiers[i]);
            }
        }

       
        
    }
}