using UnityEngine;


namespace Core.NPC.Util
{
    public enum NPCActions
    {
        idle, normal_talk, angry_talk
    }

    public class NPCAnimations
    {
        public static readonly int Idle = Animator.StringToHash("idle");
        public static readonly int TalkCalm = Animator.StringToHash("talk_calm");
        public static readonly int TalkAngry = Animator.StringToHash("talk_angry");

        public static int ReturnHash(NPCActions npcAction)
        {
            int result;
            switch (npcAction)
            {
                case NPCActions.idle:
                    result = Idle;
                    break;
                case NPCActions.normal_talk:
                    result = TalkCalm;
                    break;
                case NPCActions.angry_talk:
                    result = TalkAngry;
                    break;
                default:
                    throw new System.Exception("NPCAnimations.ReturnHash: No hash for '"
                        + npcAction.ToString() + "' enum ");
            }
            return result;
        }

    }
}
