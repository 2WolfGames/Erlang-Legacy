using System.Collections;
using DG.Tweening;
using Core.Manager;
using Core.Utility;
using UnityEngine;

namespace Core.Environment
{
    public class Door : MonoBehaviour, IOpenable, ICloseable
    {
        [SerializeField] GameObject objectDoor;
        [SerializeField] float speed = 1.0f;
        [SerializeField] Transform openPosition;
        [SerializeField] Transform closedPosition;
        [SerializeField] bool isOpen = false;
        [SerializeField] AudioClip doorMovingClip;

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
            tranformDoor.DOMoveY(openPosition.position.y, 0.75f);
            //StartCoroutine(Move(openPosition.position));
        }

        public void Close()
        {
            if (!isOpen) return;
            isOpen = false;
            tranformDoor.DOMoveY(closedPosition.position.y, 0.75f);
            //StartCoroutine(Move(closedPosition.position));
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
