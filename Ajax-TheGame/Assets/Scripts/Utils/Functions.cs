using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Utils.Enums;

namespace Utils
{
    public static class Functions
    {
        // pre: --
        // post: compute if collision is made from front or backs
        //      if collision is made in center, returns back and front randomly
        public static CollisionSide ComputeCollisionSide(Transform origin, Transform other)
        {
            Vector3 direction = origin.InverseTransformPoint(other.position);
            if (direction.x < 0)
            {
                return CollisionSide.BACK;
            }
            else if (direction.x > 0)
            {
                return CollisionSide.FRONT;
            }
            return Random.Range(0, 2) == 0 ? CollisionSide.BACK : CollisionSide.FRONT;
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

}

