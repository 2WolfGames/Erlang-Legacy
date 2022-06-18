using System.Collections;
using Core.Shared.Enum;
using UnityEngine;

public class KeyIndicatorTrigger : MonoBehaviour
{
    [SerializeField] GameKey gameKey;
    [SerializeField] string function;
    [SerializeField] float waitTimeBeforeShowingKeys;
    private bool itsNeeded = true;
    private bool playerIn = false;

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

    private void Update()
    {
        if (playerIn && Input.GetKeyDown(gameKey.ToString().ToLower()))
        {
            itsNeeded = false;
            playerIn = false; //we dont care anymore
            KeyIndicatorDisposer.Instance?.HideTutorial();
        }
    }

    IEnumerator ShowKeyIndication()
    {
        yield return new WaitForSeconds(waitTimeBeforeShowingKeys);

        if (itsNeeded && playerIn){
            KeyIndicatorDisposer.Instance?.ShowTutorial(gameKey, function);
        }
    }
}
