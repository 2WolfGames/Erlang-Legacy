using UnityEngine;

namespace Core.Combat.IA.Action
{
    // description:
    //  trigger dash over player
    public class Assault : EnemyAction
    {
        [SerializeField] float power;

        public override void OnStart()
        {
            DoAssault();
        }

        private void DoAssault()
        {
            var direction = player.transform.position.x < transform.position.x ? -1 : 1;
            body.AddForce(Vector2.right * direction, 0);
        }
    }

}
