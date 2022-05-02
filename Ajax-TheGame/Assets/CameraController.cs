using Cinemachine;
using Core.Player.Controller;
using UnityEngine;

public class PlayerAttacher : MonoBehaviour
{
    void Start()
    {
        var player = PlayerController.Instance;
        var camera = GetComponent<CinemachineVirtualCamera>();
        camera.Follow = player.transform;
    }
}
