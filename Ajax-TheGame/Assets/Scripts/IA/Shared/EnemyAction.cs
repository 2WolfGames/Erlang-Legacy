using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.Character.Player;
using UnityEngine;

namespace Core.IA.Shared
{
    public class EnemyAction : Action
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
