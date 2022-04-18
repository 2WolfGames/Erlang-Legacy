using Core.Player.Controller;
using UnityEngine;

namespace Core.Combat.IA
{
    public class EnemyAction : BehaviorDesigner.Runtime.Tasks.Action
    {
        protected Rigidbody2D body;
        protected Animator animator;
        protected PlayerController player;

        public override void OnAwake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            player = PlayerController.Instance;
        }
    }
}
