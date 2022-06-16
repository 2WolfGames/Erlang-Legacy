using System.Collections;
using Core.Shared.Enum;
using UnityEngine;

public class KeyIndicatorTrigger : MonoBehaviour
{
    [SerializeField] GameKey gameKey;
    [SerializeField] string function; 
    [SerializeField] float waitTimeBeforeShowingKeys;

    private KeyIndicatorDisposer currentkeyIndicatorDisposer;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "KeyIndicatorDisposer"){
            Debug.Log("1");
            currentkeyIndicatorDisposer = other.gameObject.GetComponent<KeyIndicatorDisposer>(); 
            StartCoroutine(ShowKeyIndication());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "KeyIndicatorDisposer" && currentkeyIndicatorDisposer != null){
            currentkeyIndicatorDisposer = null;
        }
    }

    IEnumerator ShowKeyIndication(){
        yield return new WaitForSeconds(waitTimeBeforeShowingKeys);
        currentkeyIndicatorDisposer?.ShowTutorial(gameKey,function);
    }
}
