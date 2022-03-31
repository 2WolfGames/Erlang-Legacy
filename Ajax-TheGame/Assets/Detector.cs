using System.Collections;
using System.Collections.Generic;
using Core.Character.Player;
using UnityEngine;

public class Detector : MonoBehaviour
{
    BasePlayer player;

    public bool InsideArea
    {
        get;
        private set;
    }

    void Start()
    {
        player = BasePlayer.Instance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(player.gameObject.tag))
        {
            InsideArea = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(player.gameObject.tag))
        {
            InsideArea = false;
        }
    }
}
