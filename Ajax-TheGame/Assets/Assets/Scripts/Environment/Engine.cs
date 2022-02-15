using System;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite activatedSprite;
    [SerializeField] MechanismBase  mechanism;
    [SerializeField] Transform myPoint;


    // Start is called before the first frame update
    void Start()
    {
        myPoint.position = new Vector3(myPoint.position.x,myPoint.position.y,0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other);
        if (other.gameObject.layer == 9){
            mechanism.ActivateMechanism(myPoint);
        }
    }


}
