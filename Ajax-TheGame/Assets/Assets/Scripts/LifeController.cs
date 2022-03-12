using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [Tooltip("Units that represent how much life entity has")]
    [Range(10, 10000)] [SerializeField] float life = 100;


    /**
        this fn returns true when
        the state of life controller is less or equal zero

        at that moment represents who ever 
        use this class is dead
    */
    public bool TakeLife(float life)
    {
        this.life -= life;
        return this.life <= Mathf.Epsilon;
    }
}
