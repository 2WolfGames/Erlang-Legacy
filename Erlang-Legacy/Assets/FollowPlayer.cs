using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Player.Controller;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(1,1,0);

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance != null)
            transform.position = PlayerController.Instance.transform.position + offset;
    }
}
