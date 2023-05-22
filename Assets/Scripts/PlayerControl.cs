using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerControl : MonoBehaviourPun,IPunObservable
{
    //PARAMETERS
    public float XInput
    {
        get { return xInput; }
        set { xInput = value; }
    }
    public float YInput
    {
        get { return yInput; }
        set { yInput = value; }
    }
    public float FireInput
    {
        get { return fireInput; }
        set { fireInput = value; }
    }
    public Vector2 AimPosition
    {
        get { return aimPosition; }
        set { aimPosition = value; }
    }
    [SerializeField] private float shootDelay = 0.1f;

    //CACHE - references for readability or speed
    Camera cam;
    [SerializeField] Projectiles bullet;
    [SerializeField] Transform bulletSpawnPos;
    PhotonView view;

    //STATE - private instance (member) variables
    private float xInput, yInput, fireInput;
    private float xOffset, yOffset, newXPos, newYPos;
    private float rotateAngle;
    private Vector2 aimPosition;
    private float T_ShootDelay;

    [SerializeField] float moveSpeed = 15f;


    //PUBLIC METHOD
    public void MovePlayer()
    {
        MovePlayer(new Vector2(XInput, YInput));
    }
    public void RotatePlayer()
    {
        RotatePlayer(AimPosition);
    }
    public void Shoot()
    {
        if (T_ShootDelay < shootDelay)
        {
            T_ShootDelay += Time.deltaTime;
        }
        else
        {
            T_ShootDelay = 0f;
            Shoot(FireInput);
        }

    }
    //PRIVATE METHOD
    private void Awake()
    {
        cam = Camera.main;
        T_ShootDelay = shootDelay;
        view = GetComponent<PhotonView>();
    }
    private void MovePlayer(Vector2 playerInput)
    {
        //Find the X,Y offset player need to move base on input
        xOffset = playerInput.x * moveSpeed * Time.deltaTime;
        yOffset = playerInput.y * moveSpeed * Time.deltaTime;

        //Calculate new Position base on that offset
        newXPos = transform.position.x + xOffset;
        newYPos = transform.position.y + yOffset;

        Vector3 movementVector = new Vector3(newXPos, newYPos, 0);

        //Move player
        transform.position = movementVector;
    }
    private void RotatePlayer(Vector2 aimPosition)
    {
        //Find Mouse Position
        Vector2 worldPos = cam.ScreenToWorldPoint(new Vector3(aimPosition.x, aimPosition.y, cam.nearClipPlane));
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);

        //Find the angle between player and mouse position
        Vector2 rotateOffset = new Vector2((worldPos.x - playerPos.x), (worldPos.y - playerPos.y));
        rotateAngle = Mathf.Atan2(rotateOffset.x, rotateOffset.y) * 180 / Mathf.PI;

        //Rotate player to mouse position
        Vector3 rotateDirection = new Vector3(0, 0, -rotateAngle);
        transform.rotation = Quaternion.Euler(rotateDirection);


    }

    private void Shoot(float fireInput)
    {
        if (fireInput > 0.5f)
        {
            GameObject bulletClone;
            bulletClone = PhotonNetwork.Instantiate(bullet.name, bulletSpawnPos.position, Quaternion.identity);
            float bulletSpeed = bulletClone.GetComponent<Projectiles>().Speed;
            bulletClone.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(fireInput);
        }
        else
        {
            this.fireInput = (float)stream.ReceiveNext();
        }
    }
}
