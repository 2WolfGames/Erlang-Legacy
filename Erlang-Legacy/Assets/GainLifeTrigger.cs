using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Player.Util;
using Core.Player.Controller;

public class GainLifeTrigger : MonoBehaviour
{
    private bool playerIn = false;
    private bool processHasStarted = false;
    [SerializeField] GameObject onDestroyParticleEffect;

    //pre: --
    //post: if player is in range and clicks, gain life process starts.
    void Update()
    {
        if (playerIn && !processHasStarted)
        {
            if (Input.GetButton(CharacterActions.Interact))
            {
                GainLifeProcess();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerIn = false;
        }
    }

    //pre: --
    //post: player life increase and fruit of lifes disapears
    private void GainLifeProcess()
    {
        processHasStarted = true;
        PlayerController.Instance?.GainLife();
        var inst = Instantiate(onDestroyParticleEffect, transform.position,transform.rotation);
        Destroy(this.gameObject);
    }
}
