using System.Collections.Generic;
using System.Collections;
using Core.Player.Utility;
using UnityEngine.UI;
using Core.NPC.Util;
using UnityEngine;
using DG.Tweening;
using Core.Shared;
using System;
using TMPro;
using Core.Manager;
using Core.ScriptableEffect;

namespace Core.NPC
{

    public class Dialogue : MonoBehaviour
    {

        [SerializeField] Image bubble;
        [SerializeField] Image bubblePointer;
        [SerializeField] Image nextSentenceIndicator;
        [SerializeField] TextMeshProUGUI nameField;
        [SerializeField] TextMeshProUGUI conversationField;
        [SerializeField] AudioClip[] conversationClips;
        [SerializeField] VolumeSettings volumeSettings;

        private Animator animator => GetComponent<Animator>();
        private Animator npcAnimator;
        private Queue<(string, int)> phrases;
        private bool settingUp = false;
        private bool displayingSentences = false;
        private bool phraseEnded = true;
        private bool endPhrase = false;
        private Action onEndConversation = null;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            phrases = new Queue<(string, int)>();
            nameField.text = string.Empty;
            conversationField.text = string.Empty;
        }

        //pre: --
        //post: When conversation is taking place, displays next sentece 
        //      or shows all the phase without the talk effect 
        private void Update()
        {
            if (settingUp)
                return;

            if (displayingSentences & Input.GetButtonDown(CharacterActions.Interact))
            {
                if (phraseEnded)
                {
                    DisplayNextSentence();
                }
                else
                {
                    endPhrase = true;
                }
            }

        }

        //pre: npcData and npcAnimaitor != null
        //post: sets all things ready
        public void StartConversation(NPCData npcData, Animator npcAnimator, Action onEndConversation = null)
        {
            if (displayingSentences || settingUp)
                return;

            settingUp = true;

            this.npcAnimator = npcAnimator;
            this.onEndConversation = onEndConversation;

            phrases.Clear();
            for (int i = 0; i < npcData.phrases.Length; i++)
            {
                phrases.Enqueue((npcData.phrases[i],
                NPCAnimations.ReturnHash(npcData.npcActions[i])));
            }

            OpenDialogue(npcData.npcName);
        }


        //pre: nextSentenceIndicator != null
        //post: takes one sentece of the queue and display it's 
        //      if there are no senteces closes dialogue.
        private void DisplayNextSentence()
        {
            if (phrases.Count > 0)
            {
                PlayConversationClip();
                phraseEnded = false;

                nextSentenceIndicator.DOKill(); //needed if it's in de middle of DoFade
                nextSentenceIndicator.color = Function.ColorVisible(false, nextSentenceIndicator.color);

                (string, int) currentSentence = phrases.Dequeue();

                npcAnimator.SetTrigger(currentSentence.Item2);

                conversationField.text = string.Empty;
                StartCoroutine(DisplayPhrase(currentSentence.Item1));
            }
            else
            {
                CloseDialogue();
            }
        }

        private void StopSoundAfterCloseDialogue()
        {
            if (audioSource && audioSource.isPlaying)
                audioSource.Stop();
        }

        private void PlayConversationClip()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager == null)
            {
                Debug.LogWarning("SoundManager not found");
                return;
            }
            float volume = volumeSettings == null ? 1 : volumeSettings.SoundVolume;
            soundManager.PlayRandomSound(conversationClips, volume, audioSource);
        }

        //pre: --
        //post: it displays slowly the sentence 
        //      if endPhrase is activated it displays all at one time. 
        //      when sentece its displayed, also shows the nextSennteceInicator
        private IEnumerator DisplayPhrase(string currentSentence)
        {
            if (endPhrase)
            {
                phraseEnded = true;
                conversationField.text = currentSentence;
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
                conversationField.text = currentSentence.Substring(0, conversationField.text.Length + 1);
            }

            if (currentSentence.Length == conversationField.text.Length)
            {
                phraseEnded = true;
                if (phrases.Count == 0)
                {
                    nextSentenceIndicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                }
                else
                {
                    nextSentenceIndicator.transform.rotation = Quaternion.identity;
                }
                nextSentenceIndicator.DOFade(1, endPhrase ? 0f : 1f);
            }
            else
            {
                StartCoroutine(DisplayPhrase(currentSentence));
            }

            endPhrase = false;
        }

        //pre: --
        //post: opens dialogue
        private void OpenDialogue(string name)
        {
            nameField.color = Function.ColorVisible(false, nameField.color);
            nameField.text = name;
            animator.SetTrigger("open_dialogue");
        }

        //pre: --
        //post: Starts the display of the conversation
        public void OnDialogueOpened()
        {
            nameField.color = Function.ColorVisible(true, nameField.color);
            displayingSentences = true;
            settingUp = false;
            phraseEnded = false;
            DisplayNextSentence();
        }

        //pre: --
        //post: closes dialogue
        private void CloseDialogue()
        {
            settingUp = true;

            animator.SetTrigger("close_dialogue");
            npcAnimator.SetTrigger(NPCAnimations.Idle);

            nextSentenceIndicator.DOKill();
            nextSentenceIndicator.color = Function.ColorVisible(false, nextSentenceIndicator.color);

            nameField.text = string.Empty;
            conversationField.text = string.Empty;

            StopSoundAfterCloseDialogue();
        }

        //pre: --
        //post: returns variables to it's initial values and invokes onEndConversation
        public void OnDialogueClosed()
        {
            settingUp = false;
            displayingSentences = false;
            onEndConversation?.Invoke();
        }

    }
}