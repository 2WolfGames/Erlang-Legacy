using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue:MonoBehaviour
{

    [SerializeField] TextMeshProUGUI nameField;
    [SerializeField] TextMeshProUGUI converationField;
    
    private Queue<string> phrases;
    
    void Start() {
        phrases = new Queue<string>();
    }

    public void OpenDialogue(){

    }

    public void DisplayText(string npcName){
        nameField.text = npcName;
    }

    public void CloseDialogue(){

    }

}
