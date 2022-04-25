using UnityEngine;

namespace Core.Util
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

        public void PlayOneShot(ParticleSystem particleSystem, Vector3 position)
        {
            if (particleSystem == null) 
                return;

            var effect = Instantiate(particleSystem, position, Quaternion.identity);
            effect.Play();

            var duration = effect.main.duration + effect.main.startLifetime.constantMax;
            effect.gameObject.AddComponent<Disposable>().Lifetime = duration;
        }
    }
}