using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.LifeBar
{
    public class LifeBarContainer : MonoBehaviour
    {
        const float cStartFillValueAdd = 0.2f;
        //Shake
        const float cShortShakeTime = 0.3f;
        const float cNormalShakeTime = 0.5f;
        const float cShakeMinSpeed = 25;
        const float cShakeMaxSpeed = 50;
        const float cShakeMinDisplacement = 0.5f;
        const float cShakeMaxDisplacement = 1.5f;
        //Reflection
        const float cReflectPosX = 45;
        //Shadow
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
        bool transparencyTransition = false;

        //pre: --
        //post: we set the elements of our component to their init positions
        void Awake()
        {
            startPos.x = transform.localPosition.x;
            startPos.y = transform.localPosition.y;
            reflect.transform.localPosition = new Vector2(-cReflectPosX, 0);
        }

        //pre:--
        //post: we set the values of our component to initial values
        void Start()
        {
            hasLife = true;
            shake = false;
            shadowActive = false;
            filImage.fillAmount = 1;
            Image imgshadow = shadowImage.GetComponent<Image>();
            imgshadow.color = new Color(imgshadow.color.r, imgshadow.color.g, imgshadow.color.b, 0);
        }


        //pre: --
        //post: darkShadow rotated, 
        //      if Shake, Calls method Shake for shaking life
        //      if shadowActive, shadow ligths up and fades  
        private void FixedUpdate()
        {
            RotateShadow();

            if (shake)
            {
                ShakeProcess();
            }

            if (shadowActive)
            {
                ShadowTransparency();
            }

        }

        #region Public methods 

        //pre:--
        //post: returs true if life full, else false
        public bool HasLife()
        {
            return hasLife;
        }

        //pre: --
        //post: if life empty, this method fill it's
        public IEnumerator Gain()
        {
            if (!hasLife)
            {
                yield return StartCoroutine(AddingLife(cStartFillValueAdd));
                hasLife = true;
            }
            else
            {
                yield return null;
            }
        }

        //pre: --
        //post: Effect which makes the life bright 
        public IEnumerator Reflection()
        {
            reflect.transform.DOLocalMoveX(cReflectPosX, 0.25f);
            yield return new WaitForSeconds(0.25f);
            reflect.transform.localPosition = new Vector2(-cReflectPosX, 0);
        }

        //pre: --
        //post: if life full, this method empty it's
        public IEnumerator Lose()
        {
            if (hasLife)
            {
                yield return StartCoroutine(LosingLife());
                hasLife = false;
            }
            else
            {
                yield return null;
            }
        }

        //pre: --
        //post: shadow it's shown and life shakes if islastLife = true
        //      else shadow hiden 
        public IEnumerator lastLife(bool islastLife)
        {
            Image img = shadowImage.GetComponent<Image>();
            if (islastLife)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
                yield return null;
            }
            shake = islastLife;
            shadowActive = islastLife;
        }

        //pre: --
        //post: changes sprite to one (randomly chosed) of the broken ones.
        public void BrokeLife()
        {
            if (lstSprites.Count > 0)
            {
                filImage.sprite = lstSprites[UnityEngine.Random.Range(0, lstSprites.Count)];
                filImage.fillAmount = 1;
            }
            StartCoroutine(Shake(cShortShakeTime));
        }

        //pre: --
        //post: if isShaking = true, makes life shake
        //      else puts life to it's initial position    
        public void SetShake(bool isShaking)
        {
            shake = isShaking;
            if (!isShaking)
            {
                transform.localPosition = new Vector2(startPos.x, startPos.y);
            }
        }

        #endregion

        #region Effects

        //pre: --
        //post: for a sec fills more the image of life
        //      ends calling the coroutine IReflection to make life shine
        IEnumerator AddingLife(float initialFilling)
        {
            filImage.fillAmount = initialFilling;
            filImage.DOFillAmount(1, 1f).OnComplete(() =>
            {
                StartCoroutine(Reflection());
            });
            yield return new WaitForSeconds(1f + 0.25f); //total time 
        }


        //pre: --
        //post: in a sec unfills the image of life
        //      ends calling a coroutine that makes life shake
        IEnumerator LosingLife()
        {
            filImage.fillAmount = 1;
            losingLifePS.Play();
            StartCoroutine(Shake(cNormalShakeTime));
            filImage.DOFillAmount(0, 1f);

            yield return new WaitForSeconds(1f);
        }

        //pre: seconds > 0
        //post: shakes life for the number of seconds
        IEnumerator Shake(float seconds)
        {
            SetShake(true);
            yield return new WaitForSeconds(seconds);
            SetShake(false);
        }

        //pre: --
        //post: changes the position of image randomly
        private void ShakeProcess()
        {
            var speed = UnityEngine.Random.Range(cShakeMinSpeed, cShakeMaxSpeed); //how fast it shakes
            var amount = UnityEngine.Random.Range(cShakeMinDisplacement, cShakeMaxDisplacement); //how much it shakes
            transform.localPosition = new Vector2(
                startPos.x + Mathf.Sin(Time.time * speed) * amount,
                startPos.y + (Mathf.Sin(Time.time * speed) * amount)
            );
        }

        //pre: --
        //post: ligths up and fades out the Shadow Image tho make a nice visual efect
        private void ShadowTransparency()
        {
            //transparency
            if (!transparencyTransition)
            {
                Image img = shadowImage.GetComponent<Image>();
                transparencyTransition = true;
                img.DOFade(img.color.a == cMaxTransparency ? cMinTransparency : cMaxTransparency, Random.Range(1.5f, 2f)).OnComplete(() =>
                {
                    transparencyTransition = false;
                    if (!shadowActive)
                    {
                        img.DOFade(0, 1f);
                    }
                });
            }
        }

        //pre: --
        //post: rotates shadow img
        private void RotateShadow()
        {
            //rotation
            shadowImage.transform.Rotate(Vector3.forward * cRotationAmount * Time.deltaTime);
        }

        #endregion
    }
}