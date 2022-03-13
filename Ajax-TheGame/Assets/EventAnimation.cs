using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimation : MonoBehaviour
{
    [SerializeField] AjaxController ajaxController;

    public void FreezeAjax()
    {
        ajaxController.UpdateFreeze(true);
    }

    public void UnfreezeAjax()
    {
        ajaxController.UpdateFreeze(false);
    }
}
