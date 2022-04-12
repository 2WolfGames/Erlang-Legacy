using UnityEngine;

namespace Core.Util
{
    // description:
    //  component to link to object in order to make object destroy in `lifetime`
    public class Disposable : MonoBehaviour
    {
        public float lifetime = 1.0f;

        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        public static void Bind(GameObject host, float lifetime)
        {
            var disposable = host.AddComponent<Disposable>();
            disposable.lifetime = lifetime;
        }
    }
}