using System.Collections.Generic;
using Gamekit2D;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Utility
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractOnTrigger2D : MonoBehaviour
    {
        [SerializeField] bool interactEnabled = false;
        [SerializeField] bool uniqueExecutionPerObject = false;

        public Collider2DEvent OnEnter, OnExit;
        protected Collider2D m_Collider;
        public bool Interact
        {
            get => interactEnabled;
            set
            {
                interactEnabled = value;

                if (m_Collider)
                    m_Collider.enabled = interactEnabled;

                if (interactEnabled) CleanMemory();
            }
        }
        private HashSet<GameObject> memmory = new HashSet<GameObject>();

        public void Awake()
        {
            m_Collider = GetComponent<Collider2D>();
            m_Collider.isTrigger = true;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("on trigger enter");

            if (!interactEnabled)
                return;

            if (uniqueExecutionPerObject && memmory.Contains(other.gameObject)) 
                return;

            memmory.Add(other.gameObject);

            OnEnter?.Invoke(other.GetComponent<Collider2D>()); // FIXME: this is not working!!
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("on trigger exit");

            if (!interactEnabled) 
                return;

            if (memmory.Contains(other.gameObject))
                memmory.Remove(other.gameObject);

            OnExit?.Invoke(other);
        }

        public void CleanMemory()
        {
            memmory.Clear();
        }
    }
}