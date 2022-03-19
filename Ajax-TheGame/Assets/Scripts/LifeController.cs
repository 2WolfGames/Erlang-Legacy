using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [Range(1, 10000)][SerializeField] int life = 100;

    public int Life
    {
        get
        {
            return this.life;
        }
    }

    /**
        this fn returns true when
        the state of life controller is less or equal zero

        at that moment represents who ever 
        use this class is dead
    */
    public bool TakeLife(int amount)
    {
        this.life -= amount;
        return this.life <= 0;
    }

    public void AddLife(int amount)
    {
        this.life += amount;
    }
}
