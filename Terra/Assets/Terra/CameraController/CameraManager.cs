using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Player;
using UnityEngine;

namespace Terra.CameraController
{

    [Serializable]
    public sealed class CameraConfig
    { 
        public Vector3 offset;
        public Vector3 minConstraints;
        public Vector3 maxConstraints;
        [Range(0.01f, 1f)] public float smoothSpeed = 1;
    }
    public class CameraManager : MonoBehaviourSingleton<CameraManager>, IWithSetUp
    {
        [SerializeField] private CameraConfig config = new();
        
        [Foldout("Debug"), ReadOnly] [SerializeField] CameraState currentState;
        [Foldout("Debug"), ReadOnly] [SerializeField] List<CameraState> states = new ();

        // TODO: This manager isn't really good, needs refactor
        public void SetUp()
        {
            Transform playerTransform = PlayerManager.Instance.transform;

            ZoomAndWaitState zoomState = new ZoomAndWaitState(transform, playerTransform);
            FollowTargetState followTargetState = new FollowTargetState(transform, playerTransform, config);
            EmptyCameraState emptyState = new EmptyCameraState(transform, playerTransform);
            
            states.Add(zoomState);
            states.Add(followTargetState);
            states.Add(emptyState);
            
            SetState(followTargetState);
        }

        
        void LateUpdate()
        {
           currentState?.Update();
        }

        public void SetState(CameraState state)
        {
            currentState = state;
            currentState?.OnEnter();
        }
        
        public void ChangeState(CameraState state)
        {
            currentState?.OnExit();
            currentState = state;
            currentState?.OnEnter();
        }
        
        public void TearDown()
        {
            states.Clear();
        }
    }
}