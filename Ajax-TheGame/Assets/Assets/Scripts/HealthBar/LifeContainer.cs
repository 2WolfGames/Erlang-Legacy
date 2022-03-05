using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LifeContainer : MonoBehaviour
{
    [SerializeField] Image filImage;
    [SerializeField] ParticleSystem losingLifePS;
    [SerializeField] List<Sprite> lstSprites;
    [SerializeField] GameObject reflect;
    [SerializeField] float reflectStartXPos = -45;
    Vector2 startingPos; //for shake
    bool hasLife;
    bool shake;

    void Awake () {
        startingPos.x = transform.localPosition.x;
        startingPos.y = transform.localPosition.y;
        reflect.transform.localPosition = new Vector2(reflectStartXPos, 0);
    }

    void Start() {
        hasLife = true;
        shake = false;
        filImage.fillAmount = 1;
    }

    public static LifeContainer SetUpNewLife(){
        LifeContainer lc = new LifeContainer();
        lc.hasLife = false;
        lc.filImage.fillAmount = 0;
        return lc;
    }

    private void FixedUpdate() {
        if(shake){
            Shake();
        }
    }

    public bool HasLife(){
        return hasLife;
    }

    public void Add(){
        if(!hasLife){
            hasLife = true;
            StartCoroutine(IAddingLife(0.2f));
        }
    }

    public void Reflection(){
        StartCoroutine(IReflection());
    }

    public void Remove(){
        if(hasLife){
            hasLife = false;
            losingLifePS.Play();
            StartCoroutine(IRemovingLife(0f));
            StartCoroutine(IShake(0.5f));
        }
    }


    IEnumerator IAddingLife(float filling){
        filImage.fillAmount = filling;
        yield return new WaitForSeconds(0.05f);
        if (filling < 1){
            StartCoroutine(IAddingLife(filling + 0.05f));
        } else if (filling > 1){
            StartCoroutine(IReflection());
        }
    }

    IEnumerator IRemovingLife(float unfilling){
        filImage.fillAmount = 1 - unfilling;
        yield return new WaitForSeconds(0.05f);
        if (unfilling < 1){
            StartCoroutine(IRemovingLife(unfilling + 0.05f));
        }
    }

    public void BrokeLife(){
        if (lstSprites.Count>0){
            filImage.sprite = lstSprites[Random.Range(0,lstSprites.Count)];
            filImage.fillAmount = 1;
        }
        StartCoroutine(IShake(0.3f));
    }

    #region "Shake"

    public void SetShake(bool isShaking){
        shake = isShaking;
        if (!isShaking){
            transform.localPosition= new Vector2(startingPos.x, startingPos.y);
        }
    }

    IEnumerator IShake(float seconds){
        SetShake(true);
        yield return new WaitForSeconds(seconds);
        SetShake(false);
    }

    private void Shake(){
        var speed = Random.Range(25f,50f); //how fast it shakes
        var amount = Random.Range(0.5f,1.5f); //how much it shakes
        transform.localPosition = new Vector2(
            startingPos.x + Mathf.Sin(Time.time * speed) * amount,
            startingPos.y + (Mathf.Sin(Time.time * speed) * amount)
        );
    }

    #endregion

    #region Reflection

    IEnumerator IReflection(){
        reflect.transform.localPosition = new Vector2(reflect.transform.localPosition.x + 4, 0);
        yield return new WaitForSeconds(0.01f);
        if (reflect.transform.localPosition.x < 45){
            StartCoroutine(IReflection());
        } else {
            reflect.transform.localPosition = new Vector2(reflectStartXPos, 0);
        }

    }

    #endregion

}
