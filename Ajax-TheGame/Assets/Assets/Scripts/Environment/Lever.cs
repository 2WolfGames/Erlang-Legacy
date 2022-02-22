using System;
using UnityEngine;


public class Lever : MonoBehaviour
{
    [SerializeField] Sprite defaultSprite;

    [SerializeField] Sprite activatedSprite;

    [SerializeField] Transform myPoint;

    SpriteRenderer spriteRenderer;

    IMechanism mechanism;

    // Start is called before the first frame update
    void Start()
    {
        mechanism = transform.parent.gameObject.GetComponent<IMechanism>();
        myPoint.position = new Vector3(myPoint.position.x,myPoint.position.y,0);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
    }

    //pre: -
    //post: activate mechanism if collider is the player Changes sprite too
    private void OnTriggerEnter2D(Collider2D other) {
        if (mechanism.CanActivate(this) && other.gameObject.tag == "Player"){
            mechanism.Activate(this);
            ChangeSprite();
        }
    }

    //pre: - 
    //post: changes sprite if it required and if its manager calls the same method of the other engine
    public void ChangeSprite(){        
        if (spriteRenderer.sprite != activatedSprite){
            spriteRenderer.sprite = activatedSprite;
        } else {
            spriteRenderer.sprite = defaultSprite;
        }
    } 

    public Transform GetPoint(){
        return myPoint;
    }


}
