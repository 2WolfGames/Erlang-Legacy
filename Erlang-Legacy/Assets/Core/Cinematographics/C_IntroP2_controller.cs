using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Core.NPC.Util;
using Core.Shared;
using Core.Player.Controller;
using Core.Shared.Enum;
using Core.NPC;

namespace Core.Cinematographics
{
    public class C_IntroP2_controller : MonoBehaviour
    {
        public NPCData captainJava_Dialogue2_Data;
        [SerializeField] CanvasGroup blackTransition;
        [SerializeField] DialogueManager captainJava_dialogueManager;

        //pre: --
        //post: we setup everythig for the cinematographic
        void Start()
        {
            PlayerController.Instance.Animator.SetTrigger("die");
            blackTransition.DOFade(0, 4f).SetDelay(3f).OnComplete(() =>
            {
                StartFirstDialogue();
            });
        }

        //pre: --
        //post: we trigger first dialogue
        void StartFirstDialogue()
        {
            captainJava_dialogueManager.TriggerDialogue();
        }

        //pre: --
        //post: this method is called when first dialogue ends. 
        //      set ups second dialogue
        public void PositionEverythingForSecondDialogue()
        {
            PlayerController.Instance.Controllable = false;
            blackTransition.DOFade(1, 2f).OnComplete(() =>
            {
                PlayerController.Instance.Animator.SetTrigger("revive");

                captainJava_dialogueManager.npcData = captainJava_Dialogue2_Data;

                UnityEvent startGameAction = new UnityEvent();
                startGameAction.AddListener(StartGame);
                captainJava_dialogueManager.actionAtEndOfConversation = startGameAction;

                blackTransition.DOFade(0, 2f).SetDelay(1f).OnComplete(() =>
                {
                    captainJava_dialogueManager.TriggerDialogue();
                });
            });
        }

        //pre: --
        //post: this method is called when second dialogue ends. 
        //      loads first playable scene.
        void StartGame()
        {
            StartCoroutine(Loader.LoadWithDelay(SceneID.OmedIsland_Zone1, 0.1f));
        }

    }
}