using NaughtyAttributes;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.Enums;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Items;
using Terra.LootSystem;
using Terra.Managers;
using Terra.Player;
using Terra.RewardSystem;
using TMPro;
using UIExtensionPackage.UISystem.Core.Base;
using UIExtensionPackage.UISystem.Core.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI
{
    public class RewardToggle: UIObject, IWithSetup
    {
        [SerializeField] private TMP_Text rewardName;
        [SerializeField] private TMP_Text rewardDescription;

        [SerializeField] private Image rewardIcon;

        [SerializeField] private Toggle rewardToggle;
        [SerializeField] private RewardData rewardData;

        private WeaponReward weaponReward = new();
        private StatsReward statsReward = new();

        [SerializeField, ReadOnly] private RewardType rewardType;

        public RewardType RewardType { get { return rewardType; } set { rewardType = value; } }

        public bool GetToggleStatus()
        {
            return rewardToggle.isOn;
        }

        private void ChooseRewardData()
        {
            switch(rewardType)
            {
                case RewardType.Stats: 
                    statsReward.AddRandomStat();
                    LoadStatsData();
                    break;
                case RewardType.Weapon: 
                    GetRandomWeaponRandomType(); 
                    break;
                case RewardType.Effect: 
                    break;
            }
        }

        private void GetRandomWeaponRandomType()
        {
            int rand = Random.Range(0, 1);
            
            if (rand == 0)
            {
                var randomWeapon = LootManager.Instance.LootTable.GetRandomMeleeWeapon();
                LoadWeaponData(randomWeapon?.Data);
                weaponReward.MeleeWeapon = randomWeapon;
            }
            else
            {
                var randomWeapon = LootManager.Instance.LootTable.GetRandomRangedWeapon();
                LoadWeaponData(randomWeapon?.Data);
                weaponReward.RangedWeapon = randomWeapon;
            }
        }

        private void LoadWeaponData<TData>(TData data) where TData: WeaponData
        {
            rewardName.text = data.itemName;
            rewardDescription.text = data.itemDescription;
            rewardIcon.sprite = data.itemSprite;
        }

        private void LoadStatsData()
        {
            rewardName.text = statsReward.RewardName;
            rewardDescription.text = statsReward.RewardDescription;
        }

        private void LoadEffectData(EffectData effectData)
        {
            rewardName.text = effectData.effectName;
            rewardDescription.text = effectData.effectDescription;
            rewardIcon.sprite = effectData.effectIcon;  
        }

        public void SetUp()
        {
            ChooseRewardData();
        }

        public void TearDown()
        {
            
        }

        public void ApplyReward()
        {
            switch (rewardType)
            {
                case RewardType.Stats: 
                    statsReward.ApplyReward(); 
                    break;
                case RewardType.Weapon:
                    weaponReward.ApplyReward();
                    break;
                case RewardType.Effect: break;
            }
        }
    }
}