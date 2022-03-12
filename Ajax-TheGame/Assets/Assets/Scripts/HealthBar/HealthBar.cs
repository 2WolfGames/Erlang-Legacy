using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{    
    //// Const values ////
    const int cMaxLifeContainers = 9;
    const float cCoroutineShakeWait = 1.25f;
    const float cCoroutineFillLifeWait = 1f;
    const float cCoroutineReflectLifeWait = 0.35f;

    //// Serialized variables ////
    [SerializeField] GameObject lifePrefab;
    [SerializeField] GameObject emptyLifePrefab;
    [SerializeField] List<LifeContainer> lifeContainers;
    [SerializeField] ParticleSystem newLifeApearingParticleEffect;
    
    ////Global variables ////
    int totalLifes;
    int currentLifes;

    //pre: --
    //post: life containers list initialized
    void Start()
    {
        lifeContainers = new List<LifeContainer>();
    }

    #region Public Methods

    //pre: lifesIn > 0 && lifesIn <= 9
    //post: delates current life bar and initializes health bar with the number of lifesIn 
    //      all full (of life)
    public void SetUpLifes(int lifesIn){
        lifeContainers.Clear();
        for(int i = transform.childCount - 1 ; i >= 0; i-- ){
            Destroy(transform.GetChild(i).gameObject);
        }

        totalLifes = lifesIn;
        currentLifes = lifesIn;

        for(int i = 0; i < totalLifes; i++){
            var current = Instantiate(lifePrefab,transform);
            lifeContainers.Insert(0,current.GetComponentInChildren<LifeContainer>());
        }
    }

    //pre: lifesUp > 0
    //post: heals the number of lifesUp, if lifeUP's + current lifes > total lifes 
    //      fully charges lifes. 
    //      also checks if danges effect has to be desactivated 
    //      (danger effect is activated when we have only 1 life)
    public void AddLifes(int lifesUp){
        if (currentLifes < totalLifes){
            bool desactivateDangerEffect = currentLifes == 1;
            
            StartCoroutine(FillLifes(currentLifes, Mathf.Min(totalLifes,currentLifes+lifesUp) - 1, false));
            currentLifes = Mathf.Min(totalLifes,currentLifes+lifesUp);

            if(desactivateDangerEffect){
                ActivateDangerEffect(false);
            }
        }
    }

    //pre: lifesOut > 0
    //post: removes the number of lifesOut
    //      if  currentLifes -lifeOut < 0, tiggers the Die effect
    //      if  currentLifes -lifeOut == 1 life, tiggers the Danger effect
    public void RemoveLife(int lifesOut){
        if(currentLifes > 0){
            for (int i = currentLifes - 1; i >= Mathf.Max(currentLifes - lifesOut,0) ; i--){
                lifeContainers[i].Remove();
            }
            currentLifes = Mathf.Max(0,currentLifes-lifesOut);

            if(currentLifes == 1){
                ActivateDangerEffect(true);
            } else if (currentLifes == 0){
                ActivateDangerEffect(false); //we make sure that danger effect is not active anymore
                StartCoroutine(DieEffect());
            }
        }
    }

    //pre: totalLifes < 9, (life bar only supports 9 lifes)
    //post: if total lifes < 9, it adds a newer life in the bar
    //      while healing all the other lifes. 
    public void SetUpOneMoreLife(){
        if (totalLifes < cMaxLifeContainers){
            bool desactivateDangerEffect = currentLifes == 1;

            if(desactivateDangerEffect){
                ActivateDangerEffect(false);
            }
            
            StartCoroutine(FillLifes(0,totalLifes-1,true));
        }
    }

    //pre: --
    //post: Heals all the life bar
    public void HealAllLifes(){
        bool desactivateDangerEffect = currentLifes == 1;

        if(desactivateDangerEffect){
            ActivateDangerEffect(false);
        }
        
        StartCoroutine(FillLifes(0,totalLifes-1,false));
    }

    #endregion

    #region Private methods

    //pre:  startLife <= 0  && startLife > totalLifes
    //      endLifes > 0 && endLifes < totalLifes
    //post: evaluates startLife, if its full it triggers the reflection effect.
    //      if its empty it fills it
    //      when startLife is not lower than endLife, if addOne == true 
    //      the coroutine NewLife it's called to Add new life.
    private IEnumerator FillLifes(int startLife, int endLife, bool addOne) {
        if (!lifeContainers[startLife].HasLife()){
            lifeContainers[startLife].Add();
            yield return new WaitForSeconds(cCoroutineFillLifeWait);
        } else {
            lifeContainers[startLife].Reflection();
            yield return new WaitForSeconds(cCoroutineReflectLifeWait);
        }
        if (startLife < endLife){
            StartCoroutine(FillLifes(++startLife,endLife,addOne));
        } else if (addOne){
            StartCoroutine(NewLife());
        }
    }

    //pre:  totalLifes > 9, health bar fully filled
    //post: adds NewLife to LifeBar
    private IEnumerator NewLife(){
        //we add life and assume that all other lifes are fully filled 
        totalLifes++;
        currentLifes = totalLifes;
        //we instanciate an emptyLifePrefab to the place of the new life
        GameObject current = Instantiate(emptyLifePrefab,transform);
        current.transform.SetAsFirstSibling();
        // we trigger the particle effect to the empty object (so its visible) and we wait it's duration
        ParticleSystem particleEffect = Instantiate(newLifeApearingParticleEffect, current.transform);
        particleEffect.Play();
        yield return new WaitForSeconds(particleEffect.main.duration*2);
        //Destroy current empty object and instensiate the new life
        Destroy(current);
        GameObject currentLife = Instantiate(lifePrefab,transform);
        currentLife.transform.SetAsFirstSibling();
        lifeContainers.Add(currentLife.GetComponentInChildren<LifeContainer>());  
        //Life fills to make nice effect
        lifeContainers[totalLifes - 1].FillEmptyLife();        
    }

    //pre: lifeContainer[0] it's allyas the last life
    //post: Danger effect for last life is activated or desactiveted dependig on bool
    private void ActivateDangerEffect(bool activate){
        lifeContainers[0].GetComponentInChildren<LifeContainer>().lastLife(activate);
    }

    //pre: --
    //post: All life shake for a moment before breaking.
    private IEnumerator DieEffect(){
        foreach( LifeContainer life in lifeContainers ){
            life.SetShake(true);
        }
        yield return new WaitForSeconds(cCoroutineShakeWait);
        foreach( LifeContainer life in lifeContainers ){
            life.BrokeLife();
            life.SetShake(false);
        }
    }

    #endregion

}
