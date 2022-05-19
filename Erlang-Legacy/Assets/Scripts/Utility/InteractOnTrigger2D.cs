using System.Collections.Generic;
using Gamekit2D;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Utility
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractOnTrigger2D : MonoBehaviour
    {
        public bool Interact
        {
            get => interact;
            set
            {
                interact = value;
                m_Collider.enabled = interact;
            }
        }
        public Collider2DEvent OnEnter, OnExit;
        private Collider2D m_Collider;
        private bool interact = false;

        public void Awake()
        {
            m_Collider = GetComponent<Collider2D>();
            m_Collider.isTrigger = true;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!interact)
                return;

            OnEnter?.Invoke(other);
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (!interact)
                return;

            OnExit?.Invoke(other);
        }

    }
}