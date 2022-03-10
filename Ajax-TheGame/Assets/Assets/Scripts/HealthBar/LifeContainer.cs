using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class LifeContainer : MonoBehaviour
{
    [SerializeField] Image filImage;
    [SerializeField] GameObject shadowImage;
    [SerializeField] ParticleSystem losingLifePS;
    [SerializeField] List<Sprite> lstSprites;
    [SerializeField] GameObject reflect;
    [SerializeField] float reflectStartXPos = -45;
    Vector2 startingPos; //for shake
    bool hasLife;
    bool shake;
    bool shadowActive;
    bool moreTransparency = true; 

    void Awake () {
        startingPos.x = transform.localPosition.x;
        startingPos.y = transform.localPosition.y;
        reflect.transform.localPosition = new Vector2(reflectStartXPos, 0);
    }

    void Start() {
        hasLife = true;
        shake = false;
        shadowActive = false;
        filImage.fillAmount = 1;
        shadowImage.GetComponent<Image>().enabled = false;
    }

    public void HideAll(bool hide){
        filImage.enabled = !hide;
        GetComponent<Image>().enabled = !hide;

        Image[] lstImg = reflect.GetComponentsInChildren<Image>();
        foreach( Image img in lstImg){
            img.enabled = !hide;
        }

        if(!hide){
            hasLife = false;
            Add();
        } 
    }

    private void FixedUpdate() {
        RotateShadow();

        if(shake){
            Shake();
        }

        if(shadowActive){
            Shadow();
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

    public void lastLife(bool islastLife){
        Image img = shadowImage.GetComponent<Image>();
        if (islastLife){
            img.enabled = islastLife;
            img.color =  new Color(img.color.r, img.color.g ,img.color.b , 0.1f);
            moreTransparency = false;
        } else {
            StartCoroutine(IDesactivateShadow());
        }
        shake = islastLife;
        shadowActive = islastLife;
    }

    public void BrokeLife(){
        shadowImage.transform.localScale = new Vector3(100,100,100);
        if (lstSprites.Count>0){
            filImage.sprite = lstSprites[Random.Range(0,lstSprites.Count)];
            filImage.fillAmount = 1;
        }
        StartCoroutine(IShake(0.3f));
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

    #region Shadow

    private void Shadow(){
        //transparency
        Image img = shadowImage.GetComponent<Image>();
        float transparency = img.color.a;
        if (moreTransparency){
            transparency -= Time.deltaTime * 0.1f;
            moreTransparency = transparency > 0.2f; //seguir donant true
        } else {
            transparency += Time.deltaTime * 0.1f;
            moreTransparency = transparency > 0.8f; //seguir donant false 
        }
        img.color = new Color(img.color.r, img.color.g ,img.color.b , transparency);
    }

    IEnumerator IDesactivateShadow(){
        Image img = shadowImage.GetComponent<Image>();
        float transparency = img.color.a - 0.05f;
        img.color =  new Color(img.color.r, img.color.g ,img.color.b , transparency);
        yield return new WaitForSeconds(0.05f);
        if (transparency < 0.01f){
            img.enabled = false;
        } else {
            StartCoroutine(IDesactivateShadow());
        }
    }

    private void RotateShadow(){
        //rotation
        shadowImage.transform.Rotate(Vector3.forward * 50 * Time.deltaTime ); 
    }

    #endregion

}
