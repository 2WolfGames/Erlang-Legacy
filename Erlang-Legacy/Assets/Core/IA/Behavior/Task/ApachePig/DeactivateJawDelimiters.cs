using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.IA.Behavior.ApachePig
{
    public class DeactivateJawDelimiters : BehaviorDesigner.Runtime.Tasks.Action
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
                sequence.AppendCallback(() => DeactivateJawDelimiter(jaw));
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        private void DeactivateJawDelimiter(GameObject jawDelimiter)
        {
            DeactivateJaw(jawDelimiter);
            DeactivateDelimiter(jawDelimiter);
        }

        private void DeactivateDelimiter(GameObject jawDelimiter)
        {
            Transform delimiter = jawDelimiter.transform.Find("Delimiter");
            delimiter.gameObject.SetActive(false);
        }

        private void DeactivateJaw(GameObject jaw)
        {
            Animator animator = jaw.GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogError("No animator found in " + jaw.name);
                return;
            }
            animator?.SetTrigger("Open");
        }
    }
}
