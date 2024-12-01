using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LootSystem
{
    public class LootBag : MonoBehaviour
    {
        public GameObject droppedItemPrefab;
        public List<Loot> lootList = new List<Loot>();

        List<Loot> GetDroppedItems()
        {
            int randomNumber = Random.Range(1, 101);
            List<Loot> possibleItems = new List<Loot>();
            foreach (Loot item in lootList)
            {
                if (randomNumber <= item.dropChance)
                {
                    possibleItems.Add(item);
                }
            }
            return possibleItems;
        }

        public void InstantiateLoot(Vector3 spawnPosition)
        {
            List<Loot> droppedItems = GetDroppedItems();
            if (droppedItems != null)
            {
                foreach (Loot item in droppedItems)
                {
                    GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                    lootGameObject.GetComponent<SpriteRenderer>().sprite = item.lootSprite;

                    float dropForce = 200f;
                    Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    lootGameObject.GetComponent<Rigidbody>().AddForce(dropDirection * dropForce);
                }
            }
        }
    }
}