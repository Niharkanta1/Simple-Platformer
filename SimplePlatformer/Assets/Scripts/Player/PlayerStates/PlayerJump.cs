using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private int maxJumps = 2;

    public int jumpsLeft;

    protected override void InitState()
    {
        base.InitState();
        jumpsLeft = maxJumps;
    }

    public override void ExecuteState()
    {
        if (playerController.Conditions.isCollidingBelow && playerController.Force.y == 0f)
        {
            jumpsLeft = maxJumps;
        }
    }


    protected override void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
    }

    private void jump()
    {
        if (!CanJump() || jumpsLeft == 0)
        {
            return;
        }
        jumpsLeft -= 1;
        float jumpForce = Mathf.Sqrt(jumpHeight * 2f * Mathf.Abs(playerController.Gravity));
        playerController.SetVerticalForce(jumpForce);
    }

    private bool CanJump()
    {
        if (!playerController.Conditions.isCollidingBelow && jumpsLeft <= 0)
        {
            return false;
        }
        if (playerController.Conditions.isCollidingBelow && jumpsLeft <= 0)
        {
            return false;
        }
        return true;
    }
}
