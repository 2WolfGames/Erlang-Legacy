using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxTangible : MonoBehaviour
{
    [SerializeField] TangibleEnum tangible = TangibleEnum.TANGIBLE;

    int nonTangibleEvents = 0;

    public TangibleEnum Tangible
    {
        get { return tangible; }
        private set { tangible = value; }
    }

    public void OnTemporaryNonTangible(float time, System.Action onComplete = null)
    {
        StartCoroutine(NonTangibleTimeoutEnumerator(time, onComplete));
    }

    IEnumerator NonTangibleTimeoutEnumerator(float time, System.Action onComplete = null)
    {
        nonTangibleEvents++;
        Tangible = TangibleEnum.NON_TANGIBLE;
        yield return new WaitForSeconds(time);
        nonTangibleEvents--;

        if (nonTangibleEvents == 0)
        {
            Tangible = TangibleEnum.TANGIBLE;
        }

        if (onComplete != null)
        {
            onComplete();
        }
    }

    public enum TangibleEnum
    {
        TANGIBLE,
        NON_TANGIBLE
    }
}
