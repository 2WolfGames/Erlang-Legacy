using System.Collections;
using Core.Shared.Enum;
using UnityEngine;

namespace Core.UI
{
    public class KeyIndicatorTrigger : MonoBehaviour
    {
        [Tooltip("If enabled, key indicator will trigger again even if player has already accomplished the indication")]
        [SerializeField] bool repeat = false;
        [SerializeField] GameKey gameKey;
        [SerializeField] string function;
        [SerializeField] float waitTimeBeforeShowingKeys;
        private bool itsNeeded = true;
        private bool playerIn = false;

        //pre: --
        //post: if player uses key in tutorial we make it disapear
        private void Update()
        {
            if (playerIn && Input.GetKeyDown(gameKey.ToString().ToLower()))
            {
                if (!repeat)
                {
                    itsNeeded = false;
                    playerIn = false; //we dont care anymore
                }
                KeyIndicatorDisposer.Instance?.HideTutorial();
            }
        }

        //pre: 
        //post: if player is in range and tutorial didn't had shown already
        //      it start a coroutine that will show the instructions
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!itsNeeded)
                return;

            if (other.tag == "Player" && !playerIn)
            {
                playerIn = true;
                StartCoroutine(ShowKeyIndication());
            }
        }

        //pre: 
        //post: if player is out of range and tutorial it's beeing shown
        //      it hides.
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!itsNeeded)
                return;

            if (other.tag == "Player" && playerIn)
            {
                KeyIndicatorDisposer.Instance?.HideTutorial();
                playerIn = false;
            }
        }

        //pre: --
        //post: show tutorial instructions
        IEnumerator ShowKeyIndication()
        {
            yield return new WaitForSeconds(waitTimeBeforeShowingKeys);

            if (itsNeeded && playerIn)
            {
                KeyIndicatorDisposer.Instance?.ShowTutorial(gameKey, function);
            }
        }

        public void SetItsNeeded(bool itsNeeded)
        {
            this.itsNeeded = itsNeeded;
        }
    }
}