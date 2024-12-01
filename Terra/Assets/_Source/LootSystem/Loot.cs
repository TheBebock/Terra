using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LootSystem
{
    [CreateAssetMenu(menuName = "TheBebocks/Loot")]
    public class Loot : ScriptableObject
    {
        public Sprite lootSprite;
        public string lootName;
        public int dropChance;

        public Loot(string lootName, int dropChance)
        {
            this.lootName = lootName;
            this.dropChance = dropChance;
        }
    }
}