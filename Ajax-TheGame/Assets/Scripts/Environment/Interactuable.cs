using UnityEngine;



namespace Core.Environment
{
    public class Interactuable : MonoBehaviour
    {
        [SerializeField] float detectorTime = 0.2f;

        [SerializeField] string trigger = "hover";

        float detectorTimeCooldown = 0.2f;

        Animator animator;

        private bool CanInteract => detectorTime <= 0;
        public float DetectorTime { get => detectorTime; set => detectorTime = value; }
        public string Trigger { get => trigger; set => trigger = value; }

        public void Start()
        {
            animator = GetComponent<Animator>();
            detectorTimeCooldown = detectorTime;
        }

        public void Update()
        {
            if (detectorTimeCooldown > 0)
                detectorTimeCooldown -= Time.deltaTime;
        }


        // trigger animations when player pass throught this elements
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (CanInteract && other.gameObject.CompareTag("Player"))
                animator.SetTrigger(trigger);
        }
    }

}
