using Terra.Components;
using Terra.Interfaces;
using UnityEngine;

namespace Terra.Environment
{
    public class MainLight : LightComponent, IAttachListeners
    {
        [SerializeField] private Color _bossFightColor;
        
        public override void StartLightMode() { }

        protected override void OnUpdate() { }

        public override void StopLightMode() { }
        public void AttachListeners()
        {
            //Register boss fight floor start
        }

        public void DetachListeners()
        {
            //Unregister boss fight floor start
        }
    }
}
