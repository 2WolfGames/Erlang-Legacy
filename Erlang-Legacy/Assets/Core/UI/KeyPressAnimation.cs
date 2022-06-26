using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class KeyPressAnimation : MonoBehaviour
    {
        [SerializeField] GameObject buttonTopPart;
        [SerializeField] GameObject buttonBottomPart;
        [SerializeField] GameObject buttonClickEffect;
        float waitTimeBetweenAnimations = 1.5f;
        bool inAnimationProcess = false;
        Vector3 initialPosButtonTopPart;
        Vector3 initialScaleButtonClickEffect;

        //pre: -- 
        //post: initial positions are taken, so the animation can be reloaded
        void Start()
        {
            initialPosButtonTopPart = buttonTopPart.transform.localPosition;
            initialScaleButtonClickEffect = buttonClickEffect.transform.localScale;
        }

        //pre: --
        //post: if there's no animation in proces de animation in reloaded
        void Update()
        {
            if (!inAnimationProcess)
            {
                ResetAnimation();
                inAnimationProcess = true;
                ClickAnimation();
            }
        }

        //pre: --
        //post: Makes the effect of a button being pressed
        void ClickAnimation()
        {
            buttonTopPart.transform.DOLocalMove(buttonBottomPart.transform.localPosition, 1f);
            buttonClickEffect.GetComponent<Image>().DOFade(0, 0.5f);
            buttonClickEffect.transform.DOScale(buttonClickEffect.transform.localScale * 1.5f, 0.25f).OnComplete(
                () =>
                {
                    StartCoroutine(waitTillNextAnimation());
                }
            );
        }

        //pre: --
        //post: waits a number of sec to indicate that animation has to be retriggered
        IEnumerator waitTillNextAnimation()
        {
            yield return new WaitForSeconds(waitTimeBetweenAnimations);
            inAnimationProcess = false;
        }

        //pre: --
        //post: returns variables to initial values.
        void ResetAnimation()
        {
            buttonTopPart.transform.localPosition = initialPosButtonTopPart;
            buttonClickEffect.transform.localScale = initialScaleButtonClickEffect;
            buttonClickEffect.GetComponent<Image>().color += new Color(0, 0, 0, 0.5f);
        }
    }
}