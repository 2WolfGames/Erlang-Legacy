using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimation : MonoBehaviour
{
    AjaxController ajaxController;

    void Start()
    {
        ajaxController = FindObjectOfType<AjaxController>();
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
