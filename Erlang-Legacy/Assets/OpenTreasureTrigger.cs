using UnityEngine;
using Core.Player.Util;
using UnityEngine.Events;

public class OpenTreasureTrigger : MonoBehaviour
{
    [SerializeField] Sprite treasureClose;
    [SerializeField] Sprite treasureOpen;
    [SerializeField] GameObject onOpenParticleEffect;
    SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    public UnityEvent OnOpen;
    bool playerIn = false;
    bool treasureOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = treasureClose;
    }

    private void Update() {
        if (playerIn && !treasureOpened){
            if (Input.GetButton(CharacterActions.Interact))
            {
                OpenTreasure();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerIn = false;
        }
    }

    private void OpenTreasure(){
        treasureOpened = true;
        spriteRenderer.sprite = treasureOpen;
        //event
        OnOpen?.Invoke();
        Instantiate(onOpenParticleEffect, transform.position,transform.rotation);
        
    }
}
