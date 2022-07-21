using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using DG.Tweening;
using UnityEngine;

namespace Core.IA.Behavior.ApachePig
{
    public class ActivateJawDelimiters : EnemyAction
    {
        [SerializeField] GameObject leftJawDelimiter;
        [SerializeField] GameObject rightJawDelimiter;

        private List<GameObject> delimiters = new List<GameObject>();
        private DG.Tweening.Sequence sequence;

        public override void OnStart()
        {
            delimiters.Add(leftJawDelimiter);
            delimiters.Add(rightJawDelimiter);
            sequence = DOTween.Sequence();
            foreach (GameObject jaw in delimiters)
                sequence.AppendCallback(() => ActivateJawDelimiter(jaw));
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        private void ActivateJawDelimiter(GameObject jawDelimiter)
        {
            ActivateJaw(jawDelimiter);
            ActivateDelimiter(jawDelimiter);
        }

        private void ActivateDelimiter(GameObject jawDelimiter)
        {
            Transform delimiter = jawDelimiter.transform.Find("Delimiter");
            delimiter.gameObject.SetActive(true);
        }

        private void ActivateJaw(GameObject jaw)
        {
            Animator animator = jaw.GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogError("No animator found in " + jaw.name);
                return;
            }
            animator?.SetTrigger("Close");
        }
    }
}
