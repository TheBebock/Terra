using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Pickups.Definitions;
using UnityEngine;

namespace Inventory.Pickups
{
    public class Pickup : MonoBehaviour
    {
        public virtual PickupType PickupType { get; protected set; }
        public virtual void OnPickUp() {}
  
        
    }
}

