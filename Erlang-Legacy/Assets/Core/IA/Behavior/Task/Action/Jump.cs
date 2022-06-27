using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using Core.Manager;
using DG.Tweening;
using UnityEngine;

public class Jump : EnemyAction
{
    public SharedFloat horizontalForce = 5f;
    public SharedFloat jumpForce = 10f;
    public SharedFloat buildupTime = 0.5f;
    public SharedFloat jumpTime = 1f;
    public SharedString animationTriggerName = "Jump";
    public SharedBool shakeCameraOnLanding = true;
    public SharedFloat shakeCameraIntensity = 5f;
    private bool hasLanded = false;
    private Tween builupTween;
    private Tween jumpTween;


    public override void OnStart()
    {
        builupTween = DOVirtual.DelayedCall(buildupTime.Value, StartJump, false);
        animator.SetTrigger(animationTriggerName.Value);
    }

    public override TaskStatus OnUpdate()
    {
        return hasLanded ? TaskStatus.Success : TaskStatus.Running;
    }

    private void StartJump()
    {
        var direction = player.transform.position.x < transform.position.x ? -1 : 1;
        body.AddForce(new Vector2(horizontalForce.Value * direction, jumpForce.Value), ForceMode2D.Impulse);
        jumpTween = DOVirtual.DelayedCall(jumpTime.Value, () =>
        {
            hasLanded = true;
            if (shakeCameraOnLanding.Value)
                CameraManager.Instance?.Shake(shakeCameraIntensity.Value);
        }, false);
    }

    public override void OnEnd()
    {
        hasLanded = false;
        builupTween?.Kill();
        jumpTween?.Kill();
    }
}
