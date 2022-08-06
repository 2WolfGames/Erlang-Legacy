using System.Collections;
using System.Collections.Generic;
using Core.Shared;
using Core.Shared.Enum;
using UnityEngine;
using Core.NPC;

public class C_IslandTransition_controller : MonoBehaviour
{
    [SerializeField] DialogueManager captainJava_dialogueManager;
    bool transitionStarted = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
            {
                StartIslandTransition();
            }
    }

    // Update is called once per frame
    public void StartIslandTransition()
    {
        if (transitionStarted)
            return;

        transitionStarted = true;
        captainJava_dialogueManager.TriggerDialogue();
    }

    public void LoadNextScene(){
        StartCoroutine(Loader.LoadWithDelay(SceneID.MenIsland_Zone1 ,0f));
    }
}
