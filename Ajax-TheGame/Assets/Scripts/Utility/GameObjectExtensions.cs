using UnityEngine;

namespace Core.Utility
{
    public static class GameObjectExtensions
    {
        public static void Disposable(this GameObject gameObject, float timeout)
        {
            var disposable = gameObject.AddComponent<Disposable>();
            disposable.Timeout = timeout;
        }
    }
}