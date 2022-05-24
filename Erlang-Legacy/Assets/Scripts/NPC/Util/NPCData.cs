using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.NPC.Util;


[System.Serializable]
public class NPCData
{
    public string npcName;
    
    [TextArea(3,135)]
    public string[] phrases;

    public NPCActions[] npcActions;

}
