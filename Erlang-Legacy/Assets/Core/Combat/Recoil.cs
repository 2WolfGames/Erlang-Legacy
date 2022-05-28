
using BehaviorDesigner.Runtime;
using DG.Tweening;
using UnityEngine;

namespace Core.Combat
{
    [RequireComponent(typeof(BehaviorTree))]
    public class Recoil : MonoBehaviour
    {
        private BehaviorTree behaviorTree;

        public void Awake()
        {
            behaviorTree = GetComponent<BehaviorTree>();
            behaviorTree.RegisterEvent<object>("recoil", ReceivedEvent);
        }

        public void ReceivedEvent(object arg1)
        {
            Debug.Log("Recoil received");
        }

        public void Start()
        {
            DOVirtual.DelayedCall(3f, () =>
            {
                Debug.Log("Recoil");
                behaviorTree.SendEvent<int>("recoil", 5);
            });
        }

    }
}
