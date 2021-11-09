using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    protected PlayerController playerController;
    protected float horizontalInput;
    protected float verticalInput;

    protected virtual void Start()
    {
        InitState();
    }

    protected virtual void InitState()
    {
        playerController = GetComponent<PlayerController>();
    }

    public virtual void ExecuteState()
    {

    }

    public virtual void LocalInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        GetInput();
    }

    protected virtual void GetInput()
    {

    }
}
