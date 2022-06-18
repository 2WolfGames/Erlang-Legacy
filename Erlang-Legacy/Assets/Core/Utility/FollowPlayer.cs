using UnityEngine;
using Core.Player.Controller;

namespace Core.Utility
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField] Vector3 offset = new Vector3(1,1,0);

        // pre: --
        //post: positions gameObject to same place as player with the specifyed offset.
        void Update()
        {
            if (PlayerController.Instance != null)
                transform.position = PlayerController.Instance.transform.position + offset;
        }
    }
}
