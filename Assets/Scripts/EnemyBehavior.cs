using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class EnemyBehavior : MonoBehaviourPun
{
    //PARAMETERS
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float enemyHP = 50f;

    private GameObject[] players;
    private Vector3 currentPosition;
    //CACHE - references for readability or speed
    Rigidbody2D rb;
    PhotonView view;
    //STATE - private instance (member) variables
    //PUBLIC METHOD
    [PunRPC]
    public void TakeDamage(float damage)
    {
        enemyHP -= damage;

    }
    public void Die()
    {
        if (enemyHP <= 0 && view.IsMine)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
            PhotonNetwork.RemoveRPCs(view);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //PRIVATE METHOD
    private void FollowPlayer()
    {
        GameObject nearestPlayer = FindNearestPlayer();
        if(view.IsMine)
        {
            transform.position = Vector3.MoveTowards(currentPosition, nearestPlayer.transform.position, moveSpeed * Time.deltaTime);
        }
    }
    private GameObject FindNearestPlayer()
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        currentPosition = transform.position;
        foreach(GameObject t in players)
        {
            float distance = Vector3.Distance(t.transform.position, currentPosition);
            if(distance <minDist)
            {
                tMin = t;
                minDist = distance;
            }
        }
        return tMin;
    }
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            FollowPlayer();
            Die();
        }

    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }


}
