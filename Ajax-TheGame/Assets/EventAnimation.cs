using UnityEngine;

using Core.Character.Player;

public class EventAnimation : MonoBehaviour
{
    // desc: start of hit animation
    public void FreezeAjax()
    {
        var player = BasePlayer.Instance;
        player.Controllable = false;
    }

    // desc: end of hit animation
    public void UnfreezeAjax()
    {
        var player = BasePlayer.Instance;
        player.Controllable = true;
    }

    // pre: called at end of dash animation
    public void OnDashEnd()
    {
        var player = BasePlayer.Instance;
        player.GetComponent<PlayerMovementManager>().EndDash();
    }
}
