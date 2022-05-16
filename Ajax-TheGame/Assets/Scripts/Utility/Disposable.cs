﻿using UnityEngine;

namespace Core.Utility
{
    // description:
    //  component to link to object in order to make object destroy in `lifetime`
    public class Disposable : MonoBehaviour
    {
        public float Timeout { get; set; } = 1;

        // Start is called before the first frame update
        public void Start()
        {
            Destroy(gameObject, Timeout);
        }
    }
}
