using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LifeContainer : MonoBehaviour
{
    //// Const values ////
    const float cCoroutineShortWaitSeconds = 0.025f;
    const float cCoroutineWaitSeconds = 0.05f;
    //Life Filler
    const float cStartFillValueAdd = 0.2f;
    const float cStartFillValueRemove = 0f;
    const float cFillAmount = 0.05f;
    //Shake
    const float cShortShakeTime = 0.3f;
    const float cNormalShakeTime = 0.5f;
    const float cShakeMinSpeed = 25;
    const float cShakeMaxSpeed = 50;
    const float cShakeMinDisplacement = 0.5f;
    const float cShakeMaxDisplacement = 1.5f;
    //Reflection
    const float cReflectPosX = 45;
    const float cReflectVelocity = 10;
    //Shadow
    const float cTransparencyLittleFiller = 0.005f;
    const float cTransparencyNormalFiller = 0.05f;
    const float cMaxTransparency = 0.8f;
    const float cMinTransparency = 0.2f;
    const float cRotationAmount = 50;

    //// Serialized variables ////
    [SerializeField] Image filImage;
    [SerializeField] GameObject shadowImage;
    [SerializeField] ParticleSystem losingLifePS;
    [SerializeField] List<Sprite> lstSprites;
    [SerializeField] GameObject reflect;

    //Global variables ////
    Vector2 startPos; //for shake
    bool hasLife;
    bool shake;
    bool shadowActive;
    bool moreTransparency = true; 

    //pre: --
    //post: we set the elements of our component to their init positions
    void Awake () {
        startPos.x = transform.localPosition.x;
        startPos.y = transform.localPosition.y;
        reflect.transform.localPosition = new Vector2(-cReflectPosX, 0);
    }

    //pre:--
    //post: we set the values of our component to initial values
    void Start() {
        hasLife = true;
        shake = false;
        shadowActive = false;
        filImage.fillAmount = 1;
        shadowImage.GetComponent<Image>().enabled = false;
    }

    //pre: --
    //post: darkShadow rotated, 
    //      if Shake, Calls method Shake for shaking life
    //      if shadowActive, shadow ligths up and fades  
    private void FixedUpdate() {
        RotateShadow();

        if(shake){
            Shake();
        }

        if(shadowActive){
            Shadow();
        }

    }
#region Public methods 

    //pre:--
    //post: returs true if life full, else false
    public bool HasLife(){
        return hasLife;
    }

    //pre: --
    //post: if life empty, this method fill it's
    public void Add(){
        if(!hasLife){
            hasLife = true;
            StartCoroutine(IAddingLife(cStartFillValueAdd));
        }
    }

    //pre: --
    //post: Reproduces add life effect of an already full life
    public void FillEmptyLife(){
        filImage.fillAmount = 0;
        hasLife = false;
        Add();
    }

    //pre: --
    //post: Calls coroitine IReflection to show the effect 
    //which makes the life bright 
    public void Reflection(){
        StartCoroutine(IReflection());
    }
    
    //pre: --
    //post: if life full, this method empty it's
    public void Remove(){
        if(hasLife){
            hasLife = false;
            losingLifePS.Play();
            StartCoroutine(IRemovingLife(cStartFillValueRemove));
            StartCoroutine(IShake(cNormalShakeTime));
        }
    }

    //pre: --
    //post: shadow it's shown and life shakes if islastLife = true
    //      else shadow hiden and life stable using IDesactivateShadow Coroutine
    public void lastLife(bool islastLife){
        Image img = shadowImage.GetComponent<Image>();
        if (islastLife){
            img.enabled = islastLife;
            img.color =  new Color(img.color.r, img.color.g ,img.color.b , 0f);
            moreTransparency = false; // moreOpacity = true;
        } else {
            StartCoroutine(IDesactivateShadow());
        }
        shake = islastLife;
        shadowActive = islastLife;
    }

    //pre: --
    //post: changes sprite to one (randomly chosed) of the broken ones.
    public void BrokeLife(){
        if (lstSprites.Count>0){
            filImage.sprite = lstSprites[Random.Range(0,lstSprites.Count)];
            filImage.fillAmount = 1;
        }
        StartCoroutine(IShake(cShortShakeTime));
    }

    //pre: --
    //post: if isShaking = true, makes life shake
    //      else puts life to it's initial position    
    public void SetShake(bool isShaking){
        shake = isShaking;
        if (!isShaking){
            transform.localPosition= new Vector2(startPos.x, startPos.y);
        }
    }

#endregion

#region Effects

    //pre: filling > 0
    //post: every waitforseconds, fills more the image of life
    //      ends calling the coroutine IReflection to make life shine
    IEnumerator IAddingLife(float filling){
        filImage.fillAmount = filling;
        yield return new WaitForSeconds(cCoroutineWaitSeconds);
        if (filling < 1){
            StartCoroutine(IAddingLife(filling + cFillAmount));
        } else if (filling > 1){
            StartCoroutine(IReflection());
        }
    }

    //pre: filling > 0
    //post: every waitforseconds, unfills more the image of life
    IEnumerator IRemovingLife(float unfilling){
        filImage.fillAmount = 1 - unfilling;
        yield return new WaitForSeconds(cCoroutineWaitSeconds);
        if (unfilling < 1){
            StartCoroutine(IRemovingLife(unfilling + cFillAmount));
        }
    }

    //pre: seconds > 0
    //post: shakes life for the number of seconds
    IEnumerator IShake(float seconds){
        SetShake(true);
        yield return new WaitForSeconds(seconds);
        SetShake(false);
    }

    //pre: --
    //post: changes the position of image randomly
    private void Shake(){
        var speed = Random.Range(cShakeMinSpeed,cShakeMaxSpeed); //how fast it shakes
        var amount = Random.Range(cShakeMinDisplacement,cShakeMaxDisplacement); //how much it shakes
        transform.localPosition = new Vector2(
            startPos.x + Mathf.Sin(Time.time * speed) * amount,
            startPos.y + (Mathf.Sin(Time.time * speed) * amount)
        );
    }

    //pre: --
    //post: reflects life
    //      How? takes the reflect gameobject and move it's to the other side fast
    IEnumerator IReflection(){
        reflect.transform.localPosition = new Vector2(reflect.transform.localPosition.x + cReflectVelocity, 0);
        yield return new WaitForSeconds(cCoroutineShortWaitSeconds);
        if (reflect.transform.localPosition.x < cReflectPosX){
            StartCoroutine(IReflection());
        } else {
            reflect.transform.localPosition = new Vector2(-cReflectPosX, 0);
        }
    }

    //pre: --
    //post: ligths up and fades out the Shadow Image tho make a nice visual efect
    private void Shadow(){
        //transparency
        Image img = shadowImage.GetComponent<Image>();
        float transparency = img.color.a;
        if (moreTransparency){
            transparency -=  cTransparencyLittleFiller;
            moreTransparency = transparency > cMinTransparency; //seguir donant true
        } else {
            transparency += cTransparencyLittleFiller;
            moreTransparency = transparency > cMaxTransparency; //seguir donant false 
        }
        img.color = new Color(img.color.r, img.color.g ,img.color.b , transparency);
    }

    //pre: --
    //post: fades out the shadow nicely
    IEnumerator IDesactivateShadow(){
        Image img = shadowImage.GetComponent<Image>();
        float transparency = img.color.a - cTransparencyNormalFiller;
        img.color =  new Color(img.color.r, img.color.g ,img.color.b , transparency);
        yield return new WaitForSeconds(cCoroutineWaitSeconds);
        if (transparency < 0.01f){
            img.enabled = false;
        } else {
            StartCoroutine(IDesactivateShadow());
        }
    }

    //pre: --
    //post: rotates shado img
    private void RotateShadow(){
        //rotation
        shadowImage.transform.Rotate(Vector3.forward * cRotationAmount * Time.deltaTime ); 
    }

    #endregion

}
