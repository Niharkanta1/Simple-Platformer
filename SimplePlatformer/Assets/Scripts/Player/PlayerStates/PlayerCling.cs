using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCling : PlayerStates
{

    [Header("Settings")]
    [SerializeField] private float wallClingMultiplier = 0.5f;

    protected override void GetInput()
    {
        if (horizontalInput <= -0.1f || horizontalInput >= 0.1f)
        {
            WallCling();
        }

    }

    public override void ExecuteState()
    {
        ExitWallCling();
    }

    private void WallCling()
    {
        //on floor or going up..
        if (playerController.Conditions.isCollidingBelow || playerController.Force.y >= 0)
        {
            return;
        }

        if (playerController.Conditions.isCollidingLeft && horizontalInput <= -0.1f ||
                playerController.Conditions.isCollidingRight && horizontalInput >= 0.1f)
        {
            playerController.SetWallClingMultiplier(wallClingMultiplier);
            playerController.Conditions.isWallClinging = true;
        }

    }

    private void ExitWallCling()
    {
        if (playerController.Conditions.isWallClinging)
        {
            if (playerController.Conditions.isCollidingBelow || playerController.Force.y > 0)
            {
                playerController.SetWallClingMultiplier(0f);
                playerController.Conditions.isWallClinging = false;
            }

            if (playerController.isFacingRight)
            {
                //pressing key left or not pressing anything.
                if (horizontalInput <= -0.1f || horizontalInput < 0.1f)
                {
                    playerController.SetWallClingMultiplier(0f);
                    playerController.Conditions.isWallClinging = false;
                }
            }
            else
            {
                if (horizontalInput >= 0.1f || horizontalInput > -0.1f)
                {
                    playerController.SetWallClingMultiplier(0f);
                    playerController.Conditions.isWallClinging = false;
                }
            }
        }
    }
}
