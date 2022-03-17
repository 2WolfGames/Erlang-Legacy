using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable : MonoBehaviour
{
    bool canBeTouch = true;

    public bool CanBeTouch
    {
        get
        {
            return canBeTouch;
        }
    }
    public IEnumerator UntouchableForSeconds(float time = 0)
    {
        this.canBeTouch = false;
        yield return new WaitForSeconds(time);
        this.canBeTouch = true;
    }
    
    public IEnumerator CanBeTouchCoroutine(bool canBeTouch, float time = 0)
    {
        yield return new WaitForSeconds(time);
        this.canBeTouch = canBeTouch;
    }
}
