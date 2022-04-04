using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Shared.Action;
using DG.Tweening;
using UnityEngine;

public class Jump : EnemyAction
{
    [SerializeField] float xPower, yPower;
    [SerializeField] float xMaxPower, yMaxPower;
    [SerializeField] bool randomizePower;

    float count = 0.05f;

    public override void OnStart()
    {
        _Jump();
    }

    public override TaskStatus OnUpdate()
    {
        if (count > 0) count -= Time.deltaTime;
        if (count <= 0 && IsGrounded()) return TaskStatus.Success;
        return TaskStatus.Running;
    }

    private void _Jump()
    {
        var direction = player.transform.position.x < transform.position.x ? -1 : 1;
        if (randomizePower)
        {
            body.AddForce(new Vector2(Random.Range(xPower, xMaxPower) * direction, Random.Range(yPower, yMaxPower)), ForceMode2D.Impulse);
        }
        else
        {
            body.AddForce(new Vector2(xPower * direction, yPower), ForceMode2D.Impulse);
        }
    }


}
