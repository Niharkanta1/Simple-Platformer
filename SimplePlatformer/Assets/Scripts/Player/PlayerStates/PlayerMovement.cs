using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerStates
{

    [Header("Settings")]
    [SerializeField] private float speed = 10f;

    private float horizontalMovement;
    private float verticalMovement;
    private float movement;

    public override void ExecuteState()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (Mathf.Abs(horizontalMovement) > 0.1f)
        {
            movement = horizontalMovement;
        }
        else
        {
            movement = 0f;
        }
        float moveSpeed = movement * speed;
        playerController.SetHorizontalForce(moveSpeed);
    }

    protected override void GetInput()
    {
        horizontalMovement = horizontalInput;
    }

}
