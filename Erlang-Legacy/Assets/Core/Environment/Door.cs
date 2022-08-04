using System.Collections;
using Core.Manager;
using Core.Utility;
using UnityEngine;

namespace Core.Environment
{
    public class Door : MonoBehaviour, IOpenable, ICloseable
    {
        [SerializeField] private GameObject objectDoor;
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private Transform openPosition;
        [SerializeField] private Transform closedPosition;
        [SerializeField] private bool isOpen = false;
        [SerializeField] private AudioClip doorMovingClip;

        public float Speed { get => speed; set => speed = value; }

        private Transform tranformDoor;
        private AudioSource audioSource;

        private void Awake()
        {
            if (objectDoor == null)
            {
                objectDoor = gameObject;
            }

            tranformDoor = objectDoor.transform;
            audioSource = GetComponent<AudioSource>();

            if (isOpen)
            {
                tranformDoor.position = openPosition.position;
            }
            else
            {
                tranformDoor.position = closedPosition.position;
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
            SoundManager.Instance?.PlaySound(doorMovingClip, 0.2f);
            while (Vector2.Distance(tranformDoor.position, target) > 0.1f)
            {
                tranformDoor.position = Vector2.MoveTowards(tranformDoor.position, target, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
