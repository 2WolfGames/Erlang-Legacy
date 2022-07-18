using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using Core.Utility;
using UnityEngine;

namespace Core.IA.Behavior.Task.Action
{
    public class InstantiateOverArea : EnemyAction
    {
        [SerializeField] Collider2D spawnArea;
        [SerializeField] GameObject prefab;
        [SerializeField] float offset = 0f;
        [SerializeField] float disposableTimeout = 3f;

        public override TaskStatus OnUpdate()
        {
            float xPosition = player.transform.position.x;
            float yPosition = spawnArea.bounds.max.y;
            var stoneThorn = Object.Instantiate(prefab, new Vector2(xPosition, yPosition + offset), Quaternion.identity);
            stoneThorn.gameObject.Disposable(disposableTimeout);
            return TaskStatus.Success;
        }
    }
}
