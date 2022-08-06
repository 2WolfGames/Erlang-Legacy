using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Core.NPC.Util;
using Core.Shared;
using Core.Player.Controller;
using Core.Shared.Enum;
using Core.NPC;

public class C_IntroP2_controller : MonoBehaviour
{
    public NPCData captainJava_Dialogue2_Data;
    [SerializeField] CanvasGroup blackTransition;
    [SerializeField] DialogueManager captainJava_dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Instance.Animator.SetTrigger("die");
        blackTransition.DOFade(0, 4f).SetDelay(3f).OnComplete(() =>
        {
            StartFirstDialogue();
        });
    }

    void StartFirstDialogue()
    {
        captainJava_dialogueManager.TriggerDialogue();
    }

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

    void StartGame()
    {
        StartCoroutine(Loader.LoadWithDelay(SceneID.OmedIsland_Zone1, 0.1f));
    }


}
