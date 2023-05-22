using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectiles : MonoBehaviour
{
    //PARAMETERS
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 50f;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    //CACHE - references for readability or speed
    [SerializeField] private Rigidbody2D rb;
    PhotonView view;
    //STATE - private instance (member) variables
    //PUBLIC METHOD
    [PunRPC]
    public void Init(Vector2 direction)
    {
        if (view.IsMine)
        {
            rb.velocity = direction * speed;
        }
    }



    //PRIVATE METHOD
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Bullet") { return; }
        if(view.IsMine)
        {
            PhotonView pv = collision.transform.GetComponent<PhotonView>();
            pv.RPC("TakeDamage", RpcTarget.All, damage);
            view.RPC("DestroyBullet", RpcTarget.All);
        }



    }
    [PunRPC]
    private void DestroyBullet()
    {
        if (view.IsMine)
        {
            PhotonNetwork.Destroy(view.transform.gameObject);
            PhotonNetwork.RemoveRPCs(view);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            view.RPC("DestroyBullet", RpcTarget.All);
        }
    }

    private void OnEnable()
    {
        view = GetComponent<PhotonView>();
    }


}
