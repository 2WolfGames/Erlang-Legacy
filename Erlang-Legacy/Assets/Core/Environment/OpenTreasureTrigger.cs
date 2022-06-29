using UnityEngine;
using Core.Player.Util;
using UnityEngine.Events;
namespace Core.Environment
{
    public class OpenTreasureTrigger : MonoBehaviour
    {
        [SerializeField] Sprite treasureClose;
        [SerializeField] Sprite treasureOpen;
        [SerializeField] GameObject onOpenParticleEffect;
        SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
        public UnityEvent OnOpen;
        bool playerIn = false;
        bool treasureOpened = false;

        //pre: --
        //post: we assure that treasure is closed
        void Start()
        {
            spriteRenderer.sprite = treasureClose;
        }

        //pre: --
        //post: if player is in range and clicks, we open treasure
        private void Update()
        {
            if (playerIn && !treasureOpened)
            {
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

        //pre: --
        //post: OnOpen ic called and treasure remains opened
        private void OpenTreasure()
        {
            treasureOpened = true;
            spriteRenderer.sprite = treasureOpen;
            //event
            OnOpen?.Invoke();
            Instantiate(onOpenParticleEffect, transform.position, transform.rotation);

        }
    }
}