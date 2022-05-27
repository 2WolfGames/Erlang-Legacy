using Core.Player.Controller;
using UnityEngine;


public class Springboard : MonoBehaviour
{
    [Tooltip("Impulse force to add")]
    [Range(5f, 100f)] [SerializeField] float force = 10f;

    [Tooltip("Util for diff between elements of same layer")]

    [SerializeField] float desviation = 90;

    ////////////////////////////////////////////////////////////////////////////////////////////////

    Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // desc:
    //  Computes local rotation from
    //  it's nearest parent and adds
    //  deviations `phi` desviation, by default it's 90 degree
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MovementController ajaxMovement = other.GetComponent<MovementController>();

            var rad = (transform.localEulerAngles.z + desviation) * Mathf.Deg2Rad;
            var xForce = force * Mathf.Cos(rad);
            var yForce = force * Mathf.Sin(rad);

            ajaxMovement.Impulse(new Vector2(xForce, yForce));

            animator.SetBool("EXPAND", true);
        }

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("EXPAND", false);
        }
    }
}
