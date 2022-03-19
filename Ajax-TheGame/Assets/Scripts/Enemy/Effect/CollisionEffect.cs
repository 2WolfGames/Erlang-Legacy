using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Effect.Health;

namespace Enemy.Effect
{
    public class CollisionEffect : MonoBehaviour
    {
        [SerializeField] HealthTaker healthTakerEffect;
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                healthTakerEffect.Apply(other.gameObject);
            }
        }
    }
}
