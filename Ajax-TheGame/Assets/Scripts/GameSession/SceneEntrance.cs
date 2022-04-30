using Core.Player.Controller;
using Core.Player.Util;
using Core.Shared.Enum;
using DG.Tweening;
using UnityEngine;

namespace Core.GameSession
{
    public class SceneEntrance : MonoBehaviour
    {
        [SerializeField] Transform entrancePoint;
        [SerializeField] Transform spawnPoint;
        [SerializeField] float entranceTime = 2f;
        [SerializeField][Range(0.1f, 1f)] float entranceWaitTime = 0.5f;

        //pre: --
        //post: returns entrancePoint position
        public Vector3 GetEntrancePoint()
        {
            return entrancePoint.position;
        }

        //pre: PlayerController.Instance != null
        //post: animates player entering the current scene
        public void MakeEntrance()
        {
            OnStartEntrance();
            var player = PlayerController.Instance;
            player.transform.DOMove(entrancePoint.position, entranceTime)
                            .SetDelay(entranceWaitTime)
                            .OnComplete(
                                () =>
                                {
                                    OnEndEntrance();
                                }
                            );
        }

        //pre: PlayerController.Instance != null
        //post: sets player stats for entrance
        private void OnStartEntrance()
        {
            var player = PlayerController.Instance;
            //facing player
            PlayerFacing facing = spawnPoint.position.x - entrancePoint.position.x > 0
            ? PlayerFacing.Left : PlayerFacing.Right;
            player.SetFacing(facing);
            //positioning
            player.transform.position = spawnPoint.position;
            //starting animation of runing
            player.Animator.SetBool(CharacterAnimations.Running, true);
            //while entrance, player controll disabled
            player.Controllable = false;
        }

        //pre: PlayerController.Instance != null
        //post: sets player stats for gameplay
        private void OnEndEntrance()
        {
            var player = PlayerController.Instance;
            //starting animation of runing
            player.Animator.SetBool(CharacterAnimations.Running, false);
            //entrance ended, player can move
            player.Controllable = true;
        }
    }
}
