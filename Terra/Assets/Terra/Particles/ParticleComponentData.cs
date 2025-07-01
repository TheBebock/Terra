using UnityEngine;
using Terra.Components;
using System;

namespace Terra.Particles
{
    [Serializable]
    public struct ParticleComponentData
    {
        public ParticleComponent particleComponent;
        public Vector3 offset;
        public Quaternion rotation;
        public float scaleModifier;
        public float destroyDuration;
    }
}