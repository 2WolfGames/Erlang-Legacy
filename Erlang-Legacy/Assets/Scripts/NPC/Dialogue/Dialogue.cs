using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using Core.NPC.Util;
using System;

public class Dialogue : MonoBehaviour
{

    [SerializeField] Image bubble;
    [SerializeField] Image bubblePointer;
    [SerializeField] Image nextSentenceIndicator;
    [SerializeField] TextMeshProUGUI nameField;
    [SerializeField] TextMeshProUGUI converationField;
    private Animator animator => GetComponent<Animator>();
    private Animator npcAnimator;
    private Queue<(string, int)> phrases;
    private bool settingUp = false; 
    private bool displayingSentences = false;
    private bool phraseEnded = true;
    private bool endPhrase = false;
    private Action onEndConversation = null;

    void Start()
    {
        phrases = new Queue<(string, int)>();
        nameField.text = string.Empty;
        converationField.text = string.Empty;
    }

    void Update()
    {
        if (settingUp)
            return;

        if (displayingSentences & Input.GetKeyDown(KeyCode.S))
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

    public void DisplayText(NPCData npcData, Animator npcAnimator, Action onEndConversation = null)
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

    private void DisplayNextSentence()
    {
        if (phrases.Count > 0)
        {
            phraseEnded = false;

            nextSentenceIndicator.DOKill();
            nextSentenceIndicator.color = ColorVisible(false, nextSentenceIndicator.color);

            (string, int) currentSentence = phrases.Dequeue();

            npcAnimator.SetTrigger(currentSentence.Item2);

            converationField.text = string.Empty;
            StartCoroutine(DisplayPhrase(currentSentence.Item1));
        }
        else
        {
            CloseDialogue();
        }
    }

    private IEnumerator DisplayPhrase(string currentSentence)
    {
        if (endPhrase)
        {
            phraseEnded = true;
            converationField.text = currentSentence;
        }
        else
        {
            yield return new WaitForSeconds(0.01f);
            converationField.text = currentSentence.Substring(0, converationField.text.Length + 1);
        }

        if (currentSentence.Length == converationField.text.Length)
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

    private void OpenDialogue(string name)
    {
        nameField.color = ColorVisible(false, nameField.color);
        nameField.text = name;
        animator.SetTrigger("open_dialogue");
    }

    public void OnDialogueOpened()
    {
        nameField.color = ColorVisible(true, nameField.color);
        displayingSentences = true;
        settingUp = false;
        phraseEnded = false;
        DisplayNextSentence();
    }

    private void CloseDialogue()
    {
        settingUp = true;

        animator.SetTrigger("close_dialogue");
        npcAnimator.SetTrigger(NPCAnimations.Idle);

        nextSentenceIndicator.DOKill();
        nextSentenceIndicator.color = ColorVisible(false, nextSentenceIndicator.color);

        nameField.text = string.Empty;
        converationField.text = string.Empty;
    }

    public void OnDialogueClosed()
    {
        settingUp = false;
        displayingSentences = false;
        onEndConversation?.Invoke();
    }

    private Color ColorVisible(bool makeVisible, Color c)
    {
        if (makeVisible)
        {
            c.a = 1f;
        }
        else
        {
            c.a = 0f;
        }
        return c;
    }

}
