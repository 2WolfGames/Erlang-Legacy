using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Shared.Enum;

namespace Core.Shared
{
    public static class Function
    {
        // pre: --
        // post: compute if collision is made from front or backs
        //      if collision is made in center, returns back and front randomly
        public static Side CollisionSide(Transform origin, Transform other)
        {
            Vector3 direction = origin.InverseTransformPoint(other.position);
            if (direction.x < 0)
            {
                return Side.Back;
            }
            else if (direction.x > 0)
            {
                return Side.Front;
            }
            return Random.Range(0, 2) == 0 ? Side.Back : Side.Front;
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

        
        //pre: --
        //post: called in FixedUpdate, given a game object and its rotation makes object rotate over time.
        public static void RotateGameObject(Transform GameObjectTransform, float rotationAmount)
        {
            GameObjectTransform.Rotate(Vector3.forward * rotationAmount * Time.fixedDeltaTime);
        }
    }

}

