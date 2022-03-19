using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums = Core.Utils.Enums;

/**
    thought to know Ajax facing.

    Ajax can have 3 state, moving left, 
    moving right and not moving at all
**/

public class Orientation : MonoBehaviour
{
    Enums.Facing latestFacing;

    Enums.Input latestInput;

    public Enums.Facing LatestFacing { get { return latestFacing; } }

    public Enums.Input LatestInput { get { return latestInput; } }


    void Awake()
    {
        latestFacing = Enums.Facing.RIGHT;
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
                latestInput = Enums.Input.RIGHT;
                latestFacing = Enums.Facing.RIGHT;
            }
            else
            {
                latestInput = Enums.Input.LEFT;
                latestFacing = Enums.Facing.LEFT;
            }
        }
        else
        {
            latestInput = Enums.Input.NONE;
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
