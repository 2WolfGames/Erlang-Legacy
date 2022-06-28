
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat;
using Core.Combat.IA;
using Core.IA.Bahavior.SharedVariable;
using Core.Manager;
using UnityEngine;

public class Shoot : EnemyAction
{
    public SharedGenericList<Weapon> weapons;
    public SharedBool shakeCamera;
    public SharedFloat shakeIntensity = 1;

    public override TaskStatus OnUpdate()
    {
        foreach (var weapon in weapons.Value)
        {
            var projectile = Object.Instantiate(weapon.projectilePrefab, weapon.weaponTransform.position, weapon.weaponTransform.rotation);
            projectile.Shooter = gameObject;
            var force = new Vector2(weapon.horizontalForce * transform.localScale.x, weapon.verticalForce);
            projectile.SetForce(force);
            if (shakeCamera.Value)
                CameraManager.Instance?.ShakeCamera(shakeIntensity.Value);
        }
        return TaskStatus.Success;
    }
}
