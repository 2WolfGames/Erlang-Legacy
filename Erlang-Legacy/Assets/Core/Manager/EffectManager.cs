using Core.Utility;
using UnityEngine;


namespace Core.Manager
{
    // description:
    //  component util link to object and make instances of 
    //  particles in different position in view
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void PlayOneShot(ParticleSystem particleSystem, Transform position = null, Transform parent = null)
        {
            if (particleSystem == null || transform == null)
                return;

            var effect = Instantiate(particleSystem, position.position, Quaternion.identity);
            if (parent)
                effect.transform.SetParent(parent);
            effect.Play();

            var duration = effect.main.duration + effect.main.startLifetime.constantMax;
            effect.gameObject.Disposable(duration);
        }
    }
}
