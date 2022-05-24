using Core.Player.Controller;
using Core.Player.Util;
using Core.Shared.Enum;
using DG.Tweening;
using UnityEngine;
using System;

namespace Core.Shared
{
    public class MovePlayer : MonoBehaviour
    {
        //pre: PlayerController.Instance != null
        //post: animates player entering the current scene
        public static void Trigger(Transform targetPoint, float waitTime, PlayerFacing facing, 
        float moveTime = 0f, Action OnMoveEnded = null)
        {
            OnStartMovement(facing);
            var player = PlayerController.Instance;
            float distance = Mathf.Abs(player.transform.position.x - targetPoint.position.x);

            player.transform.DOMove(targetPoint.position, moveTime == 0f ? OptimalMovementTime(distance) : moveTime)
                .SetDelay(waitTime)
                .OnComplete(
                    () =>
                    {
                        OnEndMovement(OnMoveEnded);
                    }
                );
        }

        //pre: PlayerController.Instance != null
        //post: sets player stats for entrance
        private static void OnStartMovement(PlayerFacing facing)
        {
            var player = PlayerController.Instance;
            //facing player
            player.SetFacing(facing);
            //starting animation of runing
            player.Animator.SetBool(CharacterAnimations.Running, true);
            player.Body.velocity = Vector3.zero;
        }

        //pre: PlayerController.Instance != null
        //post: sets player stats for gameplay
        private static void OnEndMovement(Action OnMoveEnded = null)
        {
            var player = PlayerController.Instance;
            //starting animation of runing
            player.Animator.SetBool(CharacterAnimations.Running, false);

            OnMoveEnded?.Invoke();
        }


        private static float OptimalMovementTime(float distance){
            // A distance = 6 it's grate if the player lasts a secon on arrive to point
            // So...by this rule of 3 the optimalMovement yime will be (distance/6) * 1
            return distance/6;
        }
    }
}