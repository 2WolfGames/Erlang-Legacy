using Core.Combat.IA;


namespace Core.IA.Behavior.Task.Action
{
    public class Indestructible : EnemyAction
    {
        public override void OnStart()
        {
            destroyable.Indestructible = true;
        }
    }
}