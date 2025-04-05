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
        public override bool CanBeInteractedWith { get; }

        public override void Interact()
        {
            //TODO: Show upgrade panel
        }

        public override void OnInteraction()
        {
            LevelGenerationManager.Instance?.LoadNewLevel();
        }
    }
}
