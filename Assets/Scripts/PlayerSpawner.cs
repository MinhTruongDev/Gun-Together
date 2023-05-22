using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    //Spawn Range
    private float minX = -13f;
    private float maxX = 13;
    private float minY = -9f;
    private float maxY = 9f;

    Vector2 spawnPos;

    private void Start()
    {
        spawnPos = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, Quaternion.identity);
    }
}
