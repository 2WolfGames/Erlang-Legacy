using System.Collections.Generic;
using System.Collections;
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
    private bool settingUp = false; //?
    private bool displayingSentences = false;
    private bool phraseEnded = true;
    private bool endPhrase = false;

    void Start()
    {
        phrases = new Queue<string>();
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

    public void DisplayText(NPCData npcData)
    {
        if (displayingSentences || settingUp)
            return;
        
        settingUp = true;

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

            nextSentenceIndicator.DOKill();
            nextSentenceIndicator.color = ColorVisible(false, nextSentenceIndicator.color);

            string currentSentence = phrases.Dequeue();
            converationField.text = string.Empty;
            StartCoroutine(DisplayPhrase(currentSentence));
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
        nextSentenceIndicator.DOKill();
        animator.SetTrigger("close_dialogue");
        nextSentenceIndicator.color = ColorVisible(false, nextSentenceIndicator.color);
        nameField.text = string.Empty;
        converationField.text = string.Empty;
    }

    public void OnDialogueClosed(){
        displayingSentences = false;
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
