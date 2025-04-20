using System.Collections;
using System.Collections.Generic;
using Terra.LevelGeneration;
using UnityEngine;

namespace Terra.Environment
{

    /// <summary>
    /// Represents object, that on interaction ends the current level
    /// </summary>
    public class EndOfLevel : InteractableBase
    {
        public override bool CanBeInteractedWith { get; protected set; }
        
        public override void OnInteraction()
        {
            
        }
    }
}
