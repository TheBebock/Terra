using System;
using System.Collections;
using System.Collections.Generic;
using _Source.StateMachine;
using Terra.StateMachine;
using UnityEngine;

namespace Terra.CameraController
{
    [Serializable]
    public class CameraState: BaseState
    {

        protected Transform cameraTransform;
        protected Transform targetTransform;
        
        public CameraState(Transform cameraTransform, Transform targetTransform)
        {
            this.cameraTransform = cameraTransform;
            this.targetTransform = targetTransform;
        }
        public override void OnEnter()
        {
            
        }

        public override void Update()
        {
            
        }

        /// <summary>
        /// Not used
        /// </summary>
        public override void FixedUpdate() { }

        public override void OnExit()
        {
            
        }
    }
}