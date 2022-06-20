using Core.Combat.IA;


namespace Core.IA.Behavior.Task.Action
{
    public class Vulnerable : EnemyAction
    {
        public override void OnStart()
        {
            destroyable.Indestructible = false;
        }
    }
}