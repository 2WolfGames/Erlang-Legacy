using System;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] Sprite defaultSprite;

    [SerializeField] Sprite activatedSprite;

    [SerializeField] MechanismBase  mechanism;

    [SerializeField] Transform myPoint;

    [SerializeField] Engine otherEngine;
    SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        myPoint.position = new Vector3(myPoint.position.x,myPoint.position.y,0);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
    }

    //pre: -
    //post: activate mechanism if collider is the player Changes sprite too
    private void OnTriggerEnter2D(Collider2D other) {
        if (mechanism.CanMechanismActivate(myPoint) && other.gameObject.tag == "Player"){
            mechanism.ActivateMechanism(myPoint);
            ChangeSprite(true);
        }
    }

    //pre: - 
    //post: changes sprite if it required and if its manager calls the same method of the other engine
    public void ChangeSprite(bool manager){        
        if (spriteRenderer.sprite != activatedSprite){
            spriteRenderer.sprite = activatedSprite;
        } else {
            spriteRenderer.sprite = defaultSprite;
        }

        if (manager && otherEngine){
            otherEngine.ChangeSprite(false);
        }
    }


}
