using Core.Utility;
using UnityEngine;

namespace Core.IA.Behavior.FalseKnight
{
    public class Door : MonoBehaviour, IOpenable, ICloseable
    {
        [SerializeField] bool isOpen = false;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (isOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        public void Open()
        {
            isOpen = true;
            animator.SetBool("isOpen", true);
        }
        public void Close()
        {
            isOpen = false;
            animator.SetBool("isOpen", false);
        }
    }
}
