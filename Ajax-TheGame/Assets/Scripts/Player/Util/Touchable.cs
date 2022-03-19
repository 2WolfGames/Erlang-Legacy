using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.Player.Util
{
    public class Touchable : MonoBehaviour
    {
        [SerializeField] bool canBeTouch = true;

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
    }
}
