using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConditions
{
    public bool isCollidingBelow { get; set; }
    public bool isCollidingAbove { get; set; }
    public bool isCollidingRight { get; set; }
    public bool isCollidingLeft { get; set; }
    public bool isFalling { get; set; }
    public bool isWallClinging { get; set; }

    public void Reset()
    {
        isCollidingBelow = false;
        isCollidingAbove = false;
        isCollidingLeft = false;
        isCollidingRight = false;
        isFalling = false;
    }

}
