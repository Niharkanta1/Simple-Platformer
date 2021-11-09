using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConditions
{
    public bool isCollidingBelow { get; set; }
    public bool isFalling { get; set; }

    public void Reset()
    {
        isCollidingBelow = false;
        isFalling = false;
    }

}
