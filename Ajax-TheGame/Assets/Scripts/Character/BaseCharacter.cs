using System.Collections;
using System.Collections.Generic;
using Core.Shared;
using UnityEngine;

namespace Core.Character
{
    [RequireComponent(typeof(LifeController))]
    public abstract class BaseCharacter : MonoBehaviour
    {
        protected LifeController lifeController;

        void Awake()
        {
            lifeController = GetComponent<LifeController>();
            OnAwake();
        }

        protected virtual void OnAwake() { }

        protected void TakeLife(int amount)
        {
            if (lifeController.TakeLife(amount))
            {
                Die();
            }
        }

        protected void AddLife(int amount)
        {
            lifeController.AddLife(amount);
        }

        public abstract void Hit(int damage, GameObject other = null, float recoverTime = 0f);

        protected virtual void Die()
        {
            Destroy(gameObject, 1f);
        }
    }

}
