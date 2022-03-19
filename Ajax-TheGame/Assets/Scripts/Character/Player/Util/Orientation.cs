using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Shared.Enum;

namespace Core.Character.Player.Util
{
    public class Orientation : MonoBehaviour
    {
        PlayerFacing latestFacing;

        PlayerInput latestInput;

        public PlayerFacing LatestFacing
        {
            get
            {
                return latestFacing;
            }
        }

        public PlayerInput LatestInput
        {
            get
            {
                return latestInput;
            }
        }

        void Awake()
        {
            latestFacing = PlayerFacing.Right;
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
                    latestInput = PlayerInput.Right;
                    latestFacing = PlayerFacing.Right;
                }
                else
                {
                    latestInput = PlayerInput.Left;
                    latestFacing = PlayerFacing.Left;
                }
            }
            else
            {
                latestInput = PlayerInput.None;
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
}
