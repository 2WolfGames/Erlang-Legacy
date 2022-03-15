using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    thought to know Ajax facing.

    Ajax can have 3 state, moving left, 
    moving right and not moving at all
**/

public class Orientation : MonoBehaviour
{
    Utils.Facing latestFacing;

    Utils.Input latestInput;

    public Utils.Facing LatestFacing { get { return latestFacing; } }

    public Utils.Input LatestInput { get { return latestInput; } }


    void Awake()
    {
        latestFacing = Utils.Facing.RIGHT;
    }

    void Update()
    {
        // avoid listening changes when ajax is freezeda
        OrientationListener();
    }

    void OrientationListener()
    {
        float inputOrientation = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(inputOrientation) > Mathf.Epsilon)
        {
            int x = Mathf.RoundToInt(inputOrientation);

            if (x == 1)
            {
                latestInput = Utils.Input.RIGHT;
                latestFacing = Utils.Facing.RIGHT;
            }
            else
            {
                latestInput = Utils.Input.LEFT;
                latestFacing = Utils.Facing.LEFT;
            }
        }
        else
        {
            latestInput = Utils.Input.NONE;
        }
    }

    // returns; left: -1 | front: 0 | right: 1
    public int InputToNumber()
    {
        return (int)latestInput - 1;
    }

    /**
        returns; left: -1 | right: 1
    */
    public int FacingToNumber()
    {
        return (int)latestFacing - 1;
    }
}
