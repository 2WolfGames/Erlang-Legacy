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
        
        public Collider2DEvent OnEnter, OnStay, OnExit;
        protected Collider2D m_Collider;
        public bool Interact
        {
            get => interactEnabled;
            set
            {
                CleanMemory();
                interactEnabled = value;
            }
        }
        private HashSet<GameObject> memmory = new HashSet<GameObject>();

        public void Awake()
        {
            m_Collider = GetComponent<Collider2D>();
            m_Collider.isTrigger = true;
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            if (!interactEnabled)
                return;

            if (uniqueExecutionPerObject && memmory.Contains(other.gameObject))
                return;

            memmory.Add(other.gameObject);

            OnStay?.Invoke(other);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!interactEnabled)
                return;

            if (uniqueExecutionPerObject && memmory.Contains(other.gameObject))
                return;

            memmory.Add(other.gameObject);

            OnEnter?.Invoke(other);
        }

        public void OnTriggerExit2D(Collider2D other)
        {

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