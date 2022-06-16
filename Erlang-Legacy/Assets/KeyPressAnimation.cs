using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class KeyPressAnimation : MonoBehaviour
{
    [SerializeField] GameObject buttonTopPart;
    [SerializeField] GameObject buttonBottomPart;
    [SerializeField] GameObject buttonClickEffect;
    float waitTimeBetweenAnimations = 1.5f;
    bool inAnimationProcess = false;
    Vector3 initialPosButtonTopPart;
    Vector3 initialScaleButtonClickEffect;
    void Start(){
        initialPosButtonTopPart = buttonTopPart.transform.localPosition;
        initialScaleButtonClickEffect = buttonClickEffect.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inAnimationProcess){
            ResetAnimation();
            inAnimationProcess = true;
            ClickAnimation();
        }
    }

    void ClickAnimation(){
        buttonTopPart.transform.DOLocalMove(buttonBottomPart.transform.localPosition,1f);
        buttonClickEffect.GetComponent<Image>().DOFade(0,0.5f);
        buttonClickEffect.transform.DOScale(buttonClickEffect.transform.localScale * 1.5f, 0.25f).OnComplete(
            () => {
                StartCoroutine(waitTillNextAnimation());
            }
        );
    }

    IEnumerator waitTillNextAnimation(){
        yield return new WaitForSeconds(waitTimeBetweenAnimations);
        inAnimationProcess = false;
    }

    void ResetAnimation(){
        buttonTopPart.transform.localPosition = initialPosButtonTopPart;
        buttonClickEffect.transform.localScale = initialScaleButtonClickEffect;
        buttonClickEffect.GetComponent<Image>().color += new Color(0,0,0,0.5f);
    }
}
