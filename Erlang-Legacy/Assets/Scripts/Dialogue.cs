using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{

    [SerializeField] Image bubble;
    [SerializeField] Image bubblePointer;
    [SerializeField] Image nextSentenceIndicator;
    [SerializeField] TextMeshProUGUI nameField;
    [SerializeField] TextMeshProUGUI converationField;
    private Animator animator => GetComponent<Animator>();
    private Queue<string> phrases;
    private bool displayingSentences = false;
    private bool phraseEnded = true;

    void Start()
    {
        phrases = new Queue<string>();
        nameField.text = string.Empty;
        converationField.text = string.Empty;
    }

    void Update()
    {
        if (displayingSentences && phraseEnded)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                DisplayNextSentence();
            }
        }
    }

    public void DisplayText(NPCData npcData)
    {
        if (displayingSentences)
            return;

        phrases.Clear();
        foreach (string phrase in npcData.phrases)
        {
            phrases.Enqueue(phrase);
        }

        OpenDialogue(npcData.npcName);
    }

    private void DisplayNextSentence()
    {
        if (phrases.Count > 0)
        {
            phraseEnded = false;
            string currentSentence = phrases.Dequeue();

            nextSentenceIndicator.DOFade(0,0.01f);
            converationField.text = currentSentence;

            nextSentenceIndicator.DOFade(1, 1f).OnComplete(
                () =>
                {
                    phraseEnded = true;
                }
            );
        }
        else
        {
            CloseDialogue();
        }
    }

    private void OpenDialogue(string name)
    {
        displayingSentences = true;
        phraseEnded = false;
        SetChildrenVisibility(true);
        animator.SetTrigger("open_dialogue");
    }

    public void OnDialogueOpened(){
        nameField.text = name;
        DisplayNextSentence();
    }

    private void CloseDialogue()
    {
        displayingSentences = false;
        nameField.text = string.Empty;
        converationField.text = string.Empty;
        animator.SetTrigger("close_dialogue");
    }

    private void SetChildrenVisibility(bool isVisible){
        nameField.enabled = isVisible;
        converationField.enabled = isVisible;
        bubble.enabled = isVisible;
        bubblePointer.enabled = isVisible;
    }

}
