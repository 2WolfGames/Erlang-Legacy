using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Character.Player;

public class EventAnimation : MonoBehaviour
{
    BasePlayer basePlayer;

    void Start()
    {
        basePlayer = BasePlayer.Instance;
    }

    // desc: change freeze state to true
    public void FreezeAjax()
    {
        basePlayer.SetFreeze(true);
    }

    // desc: change freeze state to false
    public void UnfreezeAjax()
    {
        basePlayer.SetFreeze(false);
    }
}
