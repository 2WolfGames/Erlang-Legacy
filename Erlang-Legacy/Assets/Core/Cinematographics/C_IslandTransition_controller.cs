using Core.Shared;
using Core.Shared.Enum;
using UnityEngine;
using Core.NPC;

namespace Core.Cinematographics
{
    public class C_IslandTransition_controller : MonoBehaviour
    {
        [SerializeField] DialogueManager captainJava_dialogueManager;
        bool transitionStarted = false;

        //pre: --
        //post: on player colision starts island transition
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                StartIslandTransition();
            }
        }

        //pre: --
        //post: chacks if there is no transition started, if correct triggers Captain Java dialogue.
        public void StartIslandTransition()
        {
            if (transitionStarted)
                return;

            transitionStarted = true;
            captainJava_dialogueManager.TriggerDialogue();
        }

        //pre: --
        //post: Unity Event for the end of Captain Java speach: loads new scene.
        public void LoadNextScene()
        {
            StartCoroutine(Loader.LoadWithDelay(SceneID.MenIsland_Zone1, 0f));
        }
    }
}