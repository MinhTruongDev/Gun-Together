using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawn : MonoBehaviour
{
    //Array of random enemy
    [SerializeField] GameObject[] enemyList;
    GameObject enemyPrefab;
    GameObject[] enemyCount;

    //Spawn rate
    private float waitingForNextSpawn = 1f;
    private float theCountDown;
    private int enemyMaxCount = 14;



    //Spawn Range
    private float minX = -23f;
    private float maxX = 23f;
    private float minY = -10f;
    private float maxY = 10f;

    Vector2 spawnPos;

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            theCountDown -= Time.deltaTime;
            if (theCountDown <= 0 && CountEnemy() <= enemyMaxCount)
            {
                SpawnEnemy();
                theCountDown = waitingForNextSpawn;
            }

        }
    }
    public int CountEnemy()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy");
        return enemyCount.Length;
    }

    private void SpawnEnemy()
    {
        //Choose random enemy in enemy list
        enemyPrefab = enemyList[UnityEngine.Random.Range(0, enemyList.Length)];
        //Choose random position to spawn enemy
        spawnPos = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
        //spawn enemy
        PhotonNetwork.Instantiate(enemyPrefab.name, spawnPos, transform.rotation);
    }
}

