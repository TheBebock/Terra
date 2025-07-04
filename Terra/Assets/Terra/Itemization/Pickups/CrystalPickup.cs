using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Pickups.Definitions;
using Terra.Managers;
using Terra.Utils;
using UnityEngine;

namespace Terra.Itemization.Pickups
{
    [Serializable]
    public class CrystalPickup : Pickup<CrystalPickupData>
    {
        public override PickupType PickupType => PickupType.Crystal;
        private float DifficultyModifier = 1;

        public override void OnPickUp()
        {
            SetDifficultyMultipler();
            EconomyManager.Instance?.ModifyCurrentGoldAmount(Mathf.RoundToInt(Data.crystalAmount * DifficultyModifier));
        }
        private void SetDifficultyMultipler()
        {
            switch (GameSettings.DefaultDifficultyLevel)
            {
                case Enums.GameDifficulty.Cyberiada: DifficultyModifier = 2; break;
                case Enums.GameDifficulty.Easy: DifficultyModifier = 1.5f; break;
                case Enums.GameDifficulty.Normal: DifficultyModifier = 1; break;
            }
        }
    }
}