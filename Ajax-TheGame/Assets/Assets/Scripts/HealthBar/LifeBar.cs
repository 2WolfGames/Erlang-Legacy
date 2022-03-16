using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LifeBarAction{
    setUp, gainLife, loseLife, addNewLife
} 

public class LifeBar : MonoBehaviour
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
    Queue<(LifeBarAction,int)> pendentChanges; 
    bool modifying;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeContainers = new List<LifeContainer>();
        pendentChanges = new Queue<(LifeBarAction, int)>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!modifying && pendentChanges.Count != 0){
            StartCoroutine(ManageQueue());
        }   
    }

    #region  public methods

    public void SetUpLifes(int initialLifes){
        pendentChanges.Clear(); 
        pendentChanges.Enqueue((LifeBarAction.setUp,initialLifes));
    }

    public void GainLifes(int lifesUp){
        pendentChanges.Enqueue((LifeBarAction.gainLife,lifesUp));
    }

    public void LoseLifes(int lifesOut){
        pendentChanges.Enqueue((LifeBarAction.loseLife,lifesOut));
    }

    public void HealAllLifes(){
        pendentChanges.Enqueue((LifeBarAction.gainLife,totalLifes-currentLifes));
    }

    public void SetUpNewLife(){
        pendentChanges.Clear(); 
        pendentChanges.Enqueue((LifeBarAction.addNewLife,0));
    }

    #endregion

    IEnumerator ManageQueue(){
        if (modifying || pendentChanges.Count == 0)
            yield break;
        
        Debug.Log("In Manage Queue");
        modifying = true;

        (LifeBarAction,int) actionType = pendentChanges.Dequeue();

        switch (actionType.Item1){
            case LifeBarAction.setUp:
                yield return StartCoroutine(SetUpLifesProcess(actionType.Item2));
            break;
            case LifeBarAction.gainLife:
                yield return StartCoroutine(GainLifesProcess(actionType.Item2));
            break;
            case LifeBarAction.loseLife:
                yield return StartCoroutine(LoseLifesProcess(actionType.Item2));
            break;
            case LifeBarAction.addNewLife:

            break;
            default:
                yield return null;
            break;
        }


        modifying = false;
        Debug.Log("Out Manage Queue");
    }
    
    //pre: lifesIn > 0 && lifesIn <= 9
    //post: delates current life bar and initializes health bar with the number of lifesIn 
    //      all full (of life)
    IEnumerator SetUpLifesProcess(int initialLifes){
        Debug.Log("In SetUpLifesProcess");
        lifeContainers.Clear();
        for(int i = transform.childCount - 1 ; i >= 0; i-- ){
            Destroy(transform.GetChild(i).gameObject);
        }

        totalLifes = initialLifes;
        currentLifes = initialLifes;

        for(int i = 0; i < totalLifes; i++){
            var current = Instantiate(lifePrefab,transform);
            lifeContainers.Insert(0,current.GetComponentInChildren<LifeContainer>());
        }

        yield return null;
        Debug.Log("Out SetUpLifesProcess");
    }

    IEnumerator GainLifesProcess(int lifesUp){
        Debug.Log("In GainLifesProcess");
        if (currentLifes < totalLifes){
            bool desactivateDangerEffect = currentLifes == 1;

            int lifesUpdate =  Mathf.Min(totalLifes,currentLifes+lifesUp);

            List<IEnumerator> coroutines = new List<IEnumerator>();    
            for (int i = currentLifes; i < lifesUpdate; i++){
                coroutines.Add(lifeContainers[i].Addd());
            }

            if(desactivateDangerEffect){
                ActivateDangerEffect(false);
            }

            yield return CoroutineChaining(coroutines.ToArray());
            currentLifes = lifesUpdate;

        }
        Debug.Log("Out GainLifesProcess");
    }

    public static IEnumerator CoroutineChaining(params IEnumerator[] routines)
    {
        foreach (var item in routines)
        {
            while (item.MoveNext()) yield return item.Current;
        }
        yield break;
    }

    IEnumerator LoseLifesProcess(int lifesOut){
        Debug.Log("In LoseLifesProcess");
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
        yield return null;
        Debug.Log("Out LoseLifesProcess");
    }


    //pre:  startLife <= 0  && startLife > totalLifes
    //      endLifes > 0 && endLifes < totalLifes
    //post: evaluates startLife, if its full it triggers the reflection effect.
    //      if its empty it fills it
    //      when startLife is not lower than endLife, if addOne == true 
    //      the coroutine NewLife it's called to Add new life.
    private IEnumerator FillLifes(int startLife, int endLife, bool addOne) {
        if (!lifeContainers[startLife].HasLife()){
            yield return StartCoroutine(lifeContainers[startLife].Addd());
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

}
