using Core.Player.Controller;
using Core.Shared.Enum;
using Core.Shared;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public NPCData npcData;
    [SerializeField] Transform talkPoint;
    [SerializeField] Animator npcAnimator;
    private bool playerIn = false;
    private bool inConversation = false;

    // Update is called once per frame
    void Update()
    {
        if (playerIn && !inConversation) {
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
        inConversation = true;

        var player = PlayerController.Instance;
        player.Controllable = false;
        PlayerFacing facing = talkPoint.position.x - player.gameObject.transform.position.x > 0
        ? PlayerFacing.Right : PlayerFacing.Left;

        MovePlayer.Trigger(talkPoint,0f,facing,0,() => {

            facing = talkPoint.position.x - transform.position.x > 0
            ? PlayerFacing.Left : PlayerFacing.Right;
            player.SetFacing(facing);

            GetComponentInChildren<Dialogue>().DisplayText(npcData,npcAnimator,() =>{
                player.Controllable = true;
                inConversation = false;
            });
        });
    }


}
