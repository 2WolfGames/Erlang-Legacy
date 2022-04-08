using Core.Character.Player;
using UnityEngine;

namespace Core.Combat.IA
{
    public class EnemyConditional : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        protected Rigidbody2D body;
        protected Animator animator;
        protected BasePlayer player;

        public override void OnAwake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            player = BasePlayer.Instance;
        }
    }
}

