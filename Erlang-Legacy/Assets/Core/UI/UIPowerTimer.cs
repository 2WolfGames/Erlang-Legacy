using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Core.UI
{
    public class UIPowerTimer : MonoBehaviour
    {
        [SerializeField] Image imgContainer;
        [SerializeField] Sprite powerSprite;
        [SerializeField] Sprite loadingSprite;
        bool coolingDown;
        bool tinkeling = false;
        float waitTime;

        // pre: --
        //post: we set the powerSprite to default 
        void Start()
        {
            imgContainer.sprite = powerSprite;
        }

        //pre: --
        //post: if coolingDown is activated,
        // evey frame fills more imageContaier
        // when image if fully filled we change sprite to power 
        // using the coroutine twinkle to make it notice
        void FixedUpdate()
        {
            if (coolingDown)
            {
                imgContainer.fillAmount += 1.0f / waitTime * Time.fixedDeltaTime;

                if (imgContainer.fillAmount == 1)
                {
                    coolingDown = false;
                    imgContainer.sprite = powerSprite;
                    tinkeling = true;
                    StartCoroutine(Tinkle());
                }
            }
        }

        //pre: cooldownTime > 0
        //post: if cooling down is not activated
        //      everythink is set to activate cooldown Icon animation of imgContainer
        public void PowerUsed(float cooldownTime)
        {
            coolingDown = true;
            waitTime = cooldownTime;
            imgContainer.fillAmount = 0;
            imgContainer.sprite = loadingSprite;
            tinkeling = false;
        }

        //pre: --
        //post: makes image tinkle a few times depending on times
        //      waiting for each tinkle waitSeconds 
        IEnumerator Tinkle(int times = 3, float waitSeconds = 0.1f)
        {
            if (tinkeling)
            {
                imgContainer.fillAmount = imgContainer.fillAmount == 0 ? 1 : 0;
                yield return new WaitForSeconds(waitSeconds);
                if (tinkeling)
                {
                    if (times > 0)
                    {
                        StartCoroutine(Tinkle(--times));
                    }
                    else
                    {
                        imgContainer.fillAmount = 1;
                    }
                }
            }
            else
            {
                yield return null;
            }

        }

    }
}

