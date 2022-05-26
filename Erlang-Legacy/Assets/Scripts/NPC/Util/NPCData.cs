using UnityEngine;

namespace Core.NPC.Util
{
    [System.Serializable]
    public class NPCData
    {
        public string npcName;

        [Tooltip("Phrases and NPCActions are connected, same lenght is requaired")]
        [TextArea(3, 135)]
        public string[] phrases;
        
        [Tooltip("Phrases and NPCActions are connected, same lenght is requaired")]
        public NPCActions[] npcActions;
        
    }

}
