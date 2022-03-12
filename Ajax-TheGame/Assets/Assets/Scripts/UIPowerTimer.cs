using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerTimer : MonoBehaviour
{
    [SerializeField] Image imgContainer;
    [SerializeField] Sprite powerSprite;
    [SerializeField] Sprite loadingSprite;
    bool coolingDown;
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
        if(coolingDown){
            imgContainer.fillAmount += 1.0f/waitTime * Time.deltaTime;

            if(imgContainer.fillAmount == 1){
                coolingDown=false;
                imgContainer.sprite = powerSprite;
                StartCoroutine(Tinkle());
            }
        }
    }

    //pre: cooldownTime > 0
    //post: if cooling down is not activated
    //      everythink is set to activate cooldown Icon animation of imgContainer
    public void PowerUsed(float cooldownTime){
        if (!coolingDown){
            coolingDown = true;
            waitTime = cooldownTime;
            imgContainer.fillAmount = 0;
            imgContainer.sprite = loadingSprite;
        }
    }

    //pre: --
    //post: makes image tinkle a few times depending on times
    //      waiting for each tinkle waitSeconds 
    IEnumerator Tinkle(int times = 3, float waitSeconds = 0.1f ){
        imgContainer.fillAmount = imgContainer.fillAmount == 0 ? 1 : 0;
        yield return new WaitForSeconds(waitSeconds);
        if (times > 0){
            StartCoroutine(Tinkle(--times));
        } else {
            imgContainer.fillAmount = 1;
        }

    }

}
