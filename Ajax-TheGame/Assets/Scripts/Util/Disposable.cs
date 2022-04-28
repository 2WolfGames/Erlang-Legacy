using UnityEngine;

namespace Core.Util
{
    // description:
    //  component to link to object in order to make object destroy in `lifetime`
    public class Disposable : MonoBehaviour
    {
        public float Lifetime { get; set; } = 1;

        // Start is called before the first frame update
        public void Start()
        {
            Destroy(gameObject, Lifetime);
        }

        public static void Bind(GameObject host, float lifetime)
        {
            var disposable = host.AddComponent<Disposable>();
            disposable.Lifetime = lifetime;
        }
    }
}
