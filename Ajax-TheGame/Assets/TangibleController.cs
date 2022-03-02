using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangibleController : MonoBehaviour
{
    [SerializeField] TangibleEnum tangible = TangibleEnum.TANGIBLE;

    [SerializeField] float defaultNonTangibleTime = 0.25f;

    int nonTangibleEvents = 0;

    public TangibleEnum Tangible
    {
        get { return tangible; }
        private set { tangible = value; }
    }

    public void MakeNonTangible()
    {
        StartCoroutine(MakeNonTangibleEnumerator(defaultNonTangibleTime));
    }

    public void MakeNonTangible(float time)
    {
        StartCoroutine(MakeNonTangibleEnumerator(time));
    }

    IEnumerator MakeNonTangibleEnumerator(float time)
    {
        nonTangibleEvents++;
        Tangible = TangibleEnum.NON_TANGIBLE;
        yield return new WaitForSeconds(time);
        nonTangibleEvents--;

        if (nonTangibleEvents == 0)
        {
            Tangible = TangibleEnum.TANGIBLE;
        }
    }

    public enum TangibleEnum
    {
        TANGIBLE,
        NON_TANGIBLE
    }
}
