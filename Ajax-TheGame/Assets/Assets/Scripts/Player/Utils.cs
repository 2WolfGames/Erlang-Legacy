﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Enums;

namespace Utils
{
    public class Functions
    {
        // pre: --
        // post: compute if collision is made from front or backs
        //      if collision is made in center, returns back and front randomly
        public static CollisionSide ComputeCollisionSide(Transform origin, Transform other)
        {
            Vector3 direction = origin.InverseTransformPoint(other.position);
            if (direction.x < 0)
            {
                return Enums.CollisionSide.BACK;
            }
            else if (direction.x > 0)
            {
                return Enums.CollisionSide.FRONT;
            }
            return Random.Range(0, 2) == 0 ? Enums.CollisionSide.BACK : Enums.CollisionSide.FRONT;
        }

        // pre: --
        // post: executes coroutines in order. 
        //      coroutine n+1 waits until coroutine n ends
        public static IEnumerator CoroutineChaining(params IEnumerator[] routines)
        {
            foreach (var item in routines)
            {
                while (item.MoveNext()) yield return item.Current;
            }
            yield break;
        }
    }
    namespace Enums
    {
        public enum Input
        {
            LEFT,
            NONE,
            RIGHT
        }

        public enum Facing
        {
            LEFT,
            RIGHT
        }

        public enum CollisionSide
        {
            FRONT,
            BACK
        }
    }

}
