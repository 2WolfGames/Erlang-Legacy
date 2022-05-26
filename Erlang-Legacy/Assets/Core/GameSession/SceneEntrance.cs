using Core.Player.Controller;
using Core.Shared.Enum;
using Core.Shared;
using UnityEngine;

namespace Core.GameSession
{
    public class SceneEntrance : MonoBehaviour
    {
        [SerializeField] Transform entrancePoint;
        [SerializeField] Transform spawnPoint;
        [SerializeField] float entranceTime = 2f;
        [SerializeField] [Range(0.1f, 1f)] float entranceWaitTime = 0.5f;

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
            var player = PlayerController.Instance;
            //positioning
            player.transform.position = spawnPoint.position;
            //where to face player
            Face facing = spawnPoint.position.x - entrancePoint.position.x > 0
            ? Face.Left : Face.Right;
            //player not controllable 
            player.Controllable = false;

            MovePlayer.Trigger(entrancePoint,entranceWaitTime,facing,entranceTime,() =>{
                player.Controllable = true;
            });

        }

    }
}
