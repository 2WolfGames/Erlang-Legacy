using UnityEngine;
using Core.Player.Controller;
using Core.Player.Util;
using DG.Tweening;
using Core.Shared.Enum;

namespace Core.GameSession
{
    public class SceneEntrance : MonoBehaviour
    {
        [SerializeField] Transform entrancePoint;
        [SerializeField] Transform spawnPoint;
        [SerializeField] float entranceTime = 2f;
        [SerializeField][Range(0.1f,1f)] float entranceWaitTime = 0.5f;

        public Vector3 GetEntrancePoint()
        {
            return entrancePoint.position;
        }

        public Vector3 GetSpawnPoint()
        {
            return spawnPoint.position;
        }

        public void MakeEntrance(){
            OnStartEntrance();
            var player = PlayerController.Instance;
            player.transform.DOMove(entrancePoint.position,entranceTime)
            .SetDelay(entranceWaitTime)
            .OnComplete(
                () =>{
                    OnEndEntrance();
                    player.Controllable = true;
                }
            );
        }

        private void OnStartEntrance(){
            var player = PlayerController.Instance;
            //facing player
            PlayerFacing facing = spawnPoint.position.x - entrancePoint.position.x > 0 
            ? PlayerFacing.Left : PlayerFacing.Right;
            player.SetFacing(facing);
            //positioning
            player.transform.position = spawnPoint.position;
            //starting animation of runing
            player.Animator.SetBool(CharacterAnimations.Running,true);
            //while entrance, player controll disabled
            player.Controllable = false;
        }

        private void OnEndEntrance(){
            var player = PlayerController.Instance;
            //starting animation of runing
            player.Animator.SetBool(CharacterAnimations.Running,false);
        }
    }
}
