using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.IA.Behavior.ApachePig
{
    public class HideDelimiters : BehaviorDesigner.Runtime.Tasks.Action
    {
        [SerializeField] float hiddeSpeed = 5f;
        [SerializeField] float hiddeDuration = 5f;
        private static string delimiterTagName = "Delimiter";
        private GameObject[] delimiters;
        private bool timeoutAcomplished = false;

        public override void OnStart()
        {
            delimiters = GameObject.FindGameObjectsWithTag(delimiterTagName);
            ZeroGravityForAllDelimiters();
            DOVirtual.DelayedCall(hiddeDuration, () => timeoutAcomplished = true);
        }

        public override TaskStatus OnUpdate()
        {
            foreach (GameObject delimiter in delimiters)
                Hide(delimiter);
            return AllHidden() ? TaskStatus.Success : TaskStatus.Running;
        }

        private void Hide(GameObject delimiter)
        {
            var position = delimiter.gameObject.transform.position;
            position.y += hiddeSpeed * Time.deltaTime;
            delimiter.gameObject.transform.position = position;
        }

        private bool AllHidden()
        {
            bool none = delimiters == null || delimiters.Length == 0;
            return none || timeoutAcomplished;
        }

        private void ZeroGravityForAllDelimiters()
        {
            foreach (GameObject delimiter in delimiters)
            {
                var body = delimiter.gameObject.GetComponent<Rigidbody2D>();
                body.gravityScale = 0;
            }
        }
    }
}
