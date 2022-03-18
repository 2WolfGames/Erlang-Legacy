using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace IA.Shared
{
    public class EnemyAction : Action
    {
        protected Rigidbody2D body;
        protected Animator animator;
        protected AjaxController ajax;
        protected Temporal ajaxDetector;

        public override void OnAwake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            ajaxDetector = gameObject.GetComponentInChildren<Temporal>();
            ajax = AjaxController.Instance;
        }
    }
}
