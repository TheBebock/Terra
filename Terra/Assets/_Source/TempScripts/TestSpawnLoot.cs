using System;
using System.Collections;
using System.Collections.Generic;
using Terra.Managers;
using UnityEngine;

public class TestSpawnLoot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            LootManager.Instance?.SpawnRandomItem(transform);
        }
    }
}
