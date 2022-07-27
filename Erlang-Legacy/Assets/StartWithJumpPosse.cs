using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Player.Controller;

public class StartWithJumpPosse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Instance.Animator.SetBool("jumping",true);
        PlayerController.Instance.Animator.SetTrigger("jump");
    }

}
