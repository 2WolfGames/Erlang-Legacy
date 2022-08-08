using UnityEngine;
using UnityEngine.Events;
using Core.Player.Utility;
using Core.Utility;
using Core.Manager;

namespace Core.Environment
{
    public class OpenTreasureTrigger : MonoBehaviour
    {
        [SerializeField] Sprite treasureClose;
        [SerializeField] Sprite treasureOpen;
        [SerializeField] GameObject onOpenParticleEffect;
        [SerializeField] AudioClip openSound;
        SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
        public UnityEvent OnOpen;
        private bool playerIn = false;
        private bool treasureOpened = false;

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
            SoundManager.Instance?.PlaySound(openSound, 1f, GetComponentInChildren<AudioSource>());
            treasureOpened = true;
            spriteRenderer.sprite = treasureOpen;
            //event
            OnOpen?.Invoke();
            if (OnOpen == null)
            {
                Debug.LogWarning("OnOpen is null");
            }
            var instance = Instantiate(onOpenParticleEffect, transform.position, transform.rotation);
            instance.gameObject.Disposable(10f);
        }
    }
}
