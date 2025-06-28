using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Extensions;
using Terra.Player;
using Terra.StatisticsSystem;
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
        [SerializeField, Range(0,100)] private int _opacityPercent = 80;
        [SerializeField] private List<LabelSetupData> _labelSetupData = new();
        [Foldout("References")][SerializeField] private Transform _statLabelContainer;
        [Foldout("References")][SerializeField] private StatLabel _statLabelPrefab;
        
        [Foldout("Debug"), ReadOnly][SerializeField] private List<StatLabelData> _statLabels = new();
        
        public void AttachListeners()
        {
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
            
            OnOpacityChanged(_opacityPercent);
        }
        
        private void OnOpacityChanged(int opacityPercent)
        {
            _opacityPercent = opacityPercent;
            ForceSetObjectOpacity(_opacityPercent);
        }
        
        private void OnStatValueChanged(StatisticType statisticType, int statValue)
        {
            for (int i = 0; i < _statLabels.Count; i++)
            {
                if(_statLabels[i].statisticType != statisticType) continue;
                
                _statLabels[i].statLabel.SetDescription(statValue);
            }
        }
        
        public void Show()
        {
            _canvasGroup.alpha = 1;
        }
                
        public void ResetObjectOpacityToDefault() => ForceSetObjectOpacity(_opacityPercent);
        
        public void ForceSetObjectOpacity(int opacityPercent)
        {
            opacityPercent =  Math.Clamp(opacityPercent, 0, 100);
            _canvasGroup.alpha = opacityPercent.ToFactor();
        }
        
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
            if (PlayerStatsManager.Instance != null)
            {
                PlayerStatsManager.Instance.OnStatValueChanged -= OnStatValueChanged;
            }
        }

        protected void OnValidate()
        {
            if(!_canvasGroup) _canvasGroup = GetComponent<CanvasGroup>();
            
            if (_statLabels.Count > 0)
            {
                OnOpacityChanged(_opacityPercent);
            }
        }
    }
}
