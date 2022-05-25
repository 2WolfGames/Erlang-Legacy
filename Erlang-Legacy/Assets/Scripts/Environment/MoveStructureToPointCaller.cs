using System.Collections;
using Core.Player.Util;
using UnityEngine;

//This enum is fot the type of lever we have.
//Buton Levers only are activated once, 
//Handlers, activates when Structure it not on the point and then are desactivated. 
public enum LeverType
{
    Button, Handler
}

public class MoveStructureToPointCaller : MonoBehaviour
{
    [SerializeField] LeverType type;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite activatedSprite;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float waitTime;
    [SerializeField] MoveStructureToPoint moveStructureToPointEngine;
    [SerializeField] Rigidbody2D structure;
    [SerializeField] Transform myPoint;
    bool activated = false;
    bool playerIn = false;

    int caca = 0;
    static int cacota = 0;

    //pre: --
    //post: seting defautl sprite to gameobject
    void Start()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    //pre: -- 
    //post: if lever is activated, and structure is on myPoint
    //      activaed is set to false 
    //      if type is Handler it changes sprite. 
    //      if player in and interacts activates structure
    void Update()
    {

        if (playerIn && Input.GetButtonDown(CharacterActions.Interact))
        {

            MoveStructure();
        }
        else if (activated && IsStructureOnPoint())
        {
            if (type == LeverType.Handler)
            {
                ChangeSprite();
            }
            activated = false;
        }
    }

    //pre: --
    //post: if collider is player then playerIn = true
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIn = false;
        }
    }

    //pre: --
    //post: if moveStructure not previously activated
    //      is structure is not on Point we activate moveStructure and change the sprite to activated
    //       if structure is on point and lever is type Handler, 
    //      coroutine () is activated to show the user that platform is on the "myPoint"
    private void MoveStructure()
    {
        if (!activated)
        {
            if (!IsStructureOnPoint())
            {
                ChangeSprite();
                moveStructureToPointEngine.Activate(myPoint);
                activated = true;
            }
            else if (type == LeverType.Handler)
            {
                StartCoroutine(IActivateAndDesactivateHandler());
            }
        }
    }

    //pre: type is Handler
    //post: Active sprite is set and in waitTime time is set to default. 
    IEnumerator IActivateAndDesactivateHandler()
    {
        ChangeSprite();
        yield return new WaitForSeconds(waitTime);
        ChangeSprite();
    }

    //pre: - 
    //post: changes sprite between activated - default
    private void ChangeSprite()
    {
        Debug.Log("caca: " + caca);
        caca++;
        Debug.Log("cacota: "+ cacota);
        cacota++;
        if (spriteRenderer.sprite != activatedSprite)
        {
            spriteRenderer.sprite = activatedSprite;
        }
        else
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    //pre: --
    //post: returns true if structure is on myPoint position
    private bool IsStructureOnPoint()
    {
        return structure.transform.position == myPoint.position;
    }
}
