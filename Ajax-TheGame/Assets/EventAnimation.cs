using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Player;

public class EventAnimation : MonoBehaviour
{
    Controller ajaxController;

    void Start()
    {
        ajaxController = FindObjectOfType<Controller>();
    }

    // desc: change freeze state to true
    public void FreezeAjax()
    {
        ajaxController.SetFreeze(true);
    }

    // desc: change freeze state to false
    public void UnfreezeAjax()
    {
        ajaxController.SetFreeze(false);
    }
}
