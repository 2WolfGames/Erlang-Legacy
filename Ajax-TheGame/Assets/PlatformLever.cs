using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLever : MonoBehaviour
{

    [SerializeField] Sprite defaultSprite;

    [SerializeField] Sprite activatedSprite;
    [SerializeField] StructureForceMovement structureForceMovement;
    [SerializeField] Transform myPoint;
    [SerializeField] SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other);
        if(other.gameObject.tag == "Player"){
            structureForceMovement.Activate(myPoint);
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
}
