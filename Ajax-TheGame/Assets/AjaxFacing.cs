using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Facing
{
    LEFT,
    FRONT,
    RIGH
}

/**
    thought to know Ajax facing.

    Ajax can have 3 state, moving left, 
    moving right and not moving at all
**/

public class AjaxFacing : MonoBehaviour
{
    public Facing facing = Facing.RIGH;

    public Facing latestFacing = Facing.RIGH;

    void Update()
    {
        var inputOrientation = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(inputOrientation) > Mathf.Epsilon)
        {
            var x = Mathf.RoundToInt(inputOrientation);
            if (x == 1) facing = Facing.RIGH;
            else facing = Facing.LEFT;

            latestFacing = facing;
        }
        else
        {
            facing = Facing.FRONT;
        }
    }

    // returns; left: -1 | front: 0 | right: 1
    public int FacingToNumber()
    {
        return (int)facing - 1;
    }

    /**
        returns; left: -1 | right: 1
    */
    public int LatestFacingToNumber()
    {
        return (int)latestFacing - 1;
    }
}
