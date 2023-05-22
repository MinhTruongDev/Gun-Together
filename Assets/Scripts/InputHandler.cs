using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;


public class InputHandler : MonoBehaviour
{
    //PARAMETERS
    [SerializeField] InputAction movement;
    [SerializeField] InputAction aimPosition;
    [SerializeField] InputAction fire;
    //CACHE - references for readability or speed
    PlayerControl playerControl;
    PhotonView view;
    //STATE - private instance (member) variables
    //PUBLIC METHOD
    //PRIVATE METHOD
    private void Update()
    {
        InputController();
    }
    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        view = GetComponent<PhotonView>();
    }
    private void InputController()
    {
        if (view.IsMine)
        { 
            //read player input
            ReadMovementInput();
            ReadMousePosition();
            ReadFireInput();

            //control player base on player input
            playerControl.MovePlayer();
            playerControl.RotatePlayer();
            playerControl.Shoot();
        }

    }

    private void ReadFireInput()
    {
        playerControl.FireInput = fire.ReadValue<float>();
    }

    private void ReadMovementInput()
    {
        playerControl.XInput = movement.ReadValue<Vector2>().x;
        playerControl.YInput = movement.ReadValue<Vector2>().y;
    }

    private void ReadMousePosition()
    {
        playerControl.AimPosition = Mouse.current.position.ReadValue();
    }

    private void OnEnable()
    {
        movement.Enable();
        aimPosition.Enable();
        fire.Enable();
    }
    private void OnDisable()
    {
        movement.Disable();
        aimPosition.Disable();
        fire.Disable();
    }
}
