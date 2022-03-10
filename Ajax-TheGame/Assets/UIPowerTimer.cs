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

    // Start is called before the first frame update
    void Start()
    {
        imgContainer.sprite = powerSprite;
    }

    // Update is called once per frame
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

    public void PowerUsed(float cooldownTime){
        if (!coolingDown){
            imgContainer.sprite = loadingSprite;
            imgContainer.fillAmount = 0;
            waitTime = cooldownTime;
            coolingDown = true;
        }
    }

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
