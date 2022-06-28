using UnityEngine;

namespace Core.Util
{
    public class Rotator : MonoBehaviour
    {
        public float speed = 50.0f;
        private void Update()
        {
            var rot = transform.localRotation.eulerAngles;
            rot.z += Time.deltaTime * speed;
            transform.localRotation = Quaternion.Euler(rot);
        }
    }
}