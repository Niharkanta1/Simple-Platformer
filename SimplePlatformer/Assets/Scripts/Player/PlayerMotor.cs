using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private PlayerStates[] playerStates;

    private void Start()
    {
        playerStates = GetComponents<PlayerStates>();
    }

    private void Update()
    {
        if (playerStates.Length != 0)
        {
            foreach (PlayerStates state in playerStates)
            {
                Debug.Log("getting input...");
                state.LocalInput();
                state.ExecuteState();
            }
        }

    }
}
