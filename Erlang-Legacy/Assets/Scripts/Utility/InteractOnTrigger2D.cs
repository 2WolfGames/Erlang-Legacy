using Gamekit2D;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Utility
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractOnTrigger2D : MonoBehaviour
    {
        public Collider2DEvent OnEnter, OnStay, OnExit;
        protected Collider2D m_Collider;

        public void Awake()
        {
            m_Collider = GetComponent<Collider2D>();
            m_Collider.isTrigger = true;
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            if (!enabled)
                return;

            Debug.Log("on stay");
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!enabled)
                return;

            OnEnter?.Invoke(other);
        }

        public void OnTriggerExit2D(Collider2D other)
        {

            Debug.Log("adios");

            if (!enabled)
                return;

            OnExit?.Invoke(other);
        }
    }
}