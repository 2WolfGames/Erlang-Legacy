using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public NPCData npcData;
    [SerializeField] Transform TalkPoint;
    private bool playerIn = false;

    // Update is called once per frame
    void Update()
    {
        if (playerIn) {
            if (Input.GetKeyDown(KeyCode.S)){
                TriggerDialogue();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            playerIn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player"){
            playerIn = false;
        }
    }

    private void TriggerDialogue(){
        GetComponentInChildren<Dialogue>().DisplayText(npcData);
    }
}
