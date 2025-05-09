using System;
using System.Collections;
using System.Collections.Generic;
using Terra.Combat;
using Terra.Managers;
using Terra.Player;
using UnityEngine;

public class TestSpawnLoot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            LootManager.Instance?.SpawnRandomItem(transform);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            LootManager.Instance?.SpawnRandomPickup(new Vector3(105,101,105));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerManager.Instance.PlayerEntity.TakeDamage(1);
        }
    }
}
