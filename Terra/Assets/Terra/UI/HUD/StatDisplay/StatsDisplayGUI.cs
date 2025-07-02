using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Extensions;
using Terra.Player;
using Terra.RewardSystem;
using Terra.StatisticsSystem;
using Terra.Utils;
using UIExtensionPackage.Core.Interfaces;
using UIExtensionPackage.UISystem.Core.Base;
using UIExtensionPackage.UISystem.Core.Interfaces;
using UnityEngine;

namespace Terra.UI.HUD.StatDisplay
{
    [RequireComponent(typeof(CanvasGroup))]
    public class StatsDisplayGUI : UIObject, IWithSetup, IAttachListeners, IShowHide
    {
        [Serializable]
        internal struct LabelSetupData
        {
            public StatisticType statisticType;
            public Sprite statIcon;
            public string statShortName;
        }
        
        [Serializable]
        internal struct StatLabelData
        {
            public StatisticType statisticType;
            public StatLabel statLabel;

            public StatLabelData(StatisticType statisticType, StatLabel statLabel)
            {
                this.statisticType = statisticType;
                this.statLabel = statLabel;
            }
        }
        

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Range(0,1)] private float _opacityPercent = 0.3f;
        [SerializeField] private Color _stdTextColor = Color.white;
        [SerializeField] private Color _betterTextColor = Color.green;
        [SerializeField] private Color _worseTextColor = Color.red;
        [SerializeField] private List<LabelSetupData> _labelSetupData = new();
        [Foldout("References")][SerializeField] private Transform _statLabelContainer;
        [Foldout("References")][SerializeField] private StatLabel _statLabelPrefab;
        
        [Foldout("Debug"), ReadOnly][SerializeField] private List<StatLabelData> _statLabels = new();
        
        public void AttachListeners()
        {
            EventsAPI.Register<OnRewardSelected>(OnRewardSelected);
            EventsAPI.Register<OnRewardUnselected>(OnRewardUnselected);
            EventsAPI.Register<StatsOpacityChangedEvent>(OnOpacityChanged);
            if (PlayerStatsManager.Instance != null)
            {
                PlayerStatsManager.Instance.OnStatValueChanged += OnStatValueChanged;
            }
        }

        public void SetUp()
        {
            for (int i = 0; i < _labelSetupData.Count; i++)
            {
                StatLabel statLabel = Instantiate(_statLabelPrefab, _statLabelContainer);
                int statValue = PlayerStatsManager.Instance.GetStatValue(_labelSetupData[i].statisticType);
                statLabel.Init(_labelSetupData[i].statIcon, _labelSetupData[i].statShortName, statValue.ToString());
                _statLabels.Add(new StatLabelData(_labelSetupData[i].statisticType, statLabel));
            }

            _opacityPercent = GameSettings.DefaultStatsOpacity;
            OnOpacityChanged(_opacityPercent);
        }


        private void OnOpacityChanged(ref StatsOpacityChangedEvent opacity)
        {
            OnOpacityChanged(opacity.alfa);
        }
        private void OnStatValueChanged(StatisticType statisticType, int statValue)
        {
            for (int i = 0; i < _statLabels.Count; i++)
            {
                if(_statLabels[i].statisticType != statisticType) continue;
                
                _statLabels[i].statLabel.SetDescription(statValue);
            }
        }

        private void OnRewardSelected(ref OnRewardSelected rewardSelected)
        {
            for (int i = 0; i < _statLabels.Count; i++)
            {
                DisplayTempValue(_statLabels[i], ref rewardSelected.comparison);
            }
        }

        private void DisplayTempValue(StatLabelData statLabelData, ref StatsDataComparison comparison)
        {
            switch (statLabelData.statisticType)
            {
                case StatisticType.Strength:
                    if(comparison.strengthValue == 0) return;
                    statLabelData.statLabel.SetDescription(comparison.strengthValue);
                    statLabelData.statLabel.SetDescriptionColor(GetColorBasedOnComparison(comparison.strength));
                    break;
                case StatisticType.MaxHealth:
                    if(comparison.maxHealthValue == 0) return;

                    statLabelData.statLabel.SetDescription(comparison.maxHealthValue);
                    statLabelData.statLabel.SetDescriptionColor(GetColorBasedOnComparison(comparison.maxHealth));
                    break;
                case StatisticType.Dexterity:
                    if(comparison.dexterityValue == 0) return;

                    statLabelData.statLabel.SetDescription(comparison.dexterityValue);
                    statLabelData.statLabel.SetDescriptionColor(GetColorBasedOnComparison(comparison.dexterity));
                    break;
                case StatisticType.Luck:
                    if(comparison.luckValue == 0) return;

                    statLabelData.statLabel.SetDescription(comparison.luckValue);
                    statLabelData.statLabel.SetDescriptionColor(GetColorBasedOnComparison(comparison.luck));
                    break;
            }
        }

        private Color GetColorBasedOnComparison(Comparison comparison)
        {
            switch (comparison)
            {
                case Comparison.Worse: return _worseTextColor; 
                case Comparison.Equal: return _stdTextColor;
                case Comparison.Better: return _betterTextColor;
            }
            return _stdTextColor;
        }
        private void OnRewardUnselected(ref OnRewardUnselected dummy)
        {
            ResetValues();
        }
        private void ResetValues()
        {
            for (int i = 0; i < _statLabels.Count; i++)
            {
                int statValue = PlayerStatsManager.Instance.GetStatValue(_labelSetupData[i].statisticType);
                _statLabels[i].statLabel.SetDescription(statValue);
                _statLabels[i].statLabel.SetDescriptionColor(_stdTextColor);
            }
        }
        
        public void Show()
        {
            _canvasGroup.alpha = 1;
        }
        private void OnOpacityChanged(float opacityPercent)
        {
            _opacityPercent = Mathf.Clamp01(opacityPercent);
            ForceSetObjectOpacity(_opacityPercent);
            GameSettings.DefaultStatsOpacity = _opacityPercent;
        }
                
        private void ForceSetObjectOpacity(float opacity)
        {
            opacity =  Mathf.Clamp01(opacity);
            _canvasGroup.alpha = opacity;
        }

        public void ResetObjectOpacityToDefault() => ForceSetObjectOpacity(_opacityPercent);
        

        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }
        
        public void TearDown()
        {
            //Noop
        }
 
        public void DetachListeners()
        {
            EventsAPI.Unregister<OnRewardSelected>(OnRewardSelected);
            EventsAPI.Unregister<OnRewardUnselected>(OnRewardUnselected);
            EventsAPI.Unregister<StatsOpacityChangedEvent>(OnOpacityChanged);

            if (PlayerStatsManager.Instance != null)
            {
                PlayerStatsManager.Instance.OnStatValueChanged -= OnStatValueChanged;
            }
        }

        private void OnValidate()
        {
            if(!_canvasGroup) _canvasGroup = GetComponent<CanvasGroup>();
            
            OnOpacityChanged(_opacityPercent);
        }
    }
}
