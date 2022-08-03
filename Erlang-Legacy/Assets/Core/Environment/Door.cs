using System.Collections;
using Core.Utility;
using UnityEngine;

namespace Core.IA.Behavior.FalseKnight
{
    public class Door : MonoBehaviour, IOpenable, ICloseable
    {
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private Transform openPosition;
        [SerializeField] private Transform closedPosition;
        [SerializeField] private bool isOpen = false;

        private void Awake()
        {
            if (isOpen)
            {
                transform.position = openPosition.position;
            }
            else
            {
                transform.position = closedPosition.position;
            }
        }

        public void Open()
        {
            if (isOpen) return;
            isOpen = true;
            StartCoroutine(Move(openPosition.position));
        }

        public void Close()
        {
            if (!isOpen) return;
            isOpen = false;
            StartCoroutine(Move(closedPosition.position));
        }

        private IEnumerator Move(Vector3 target)
        {
            while (Vector2.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
