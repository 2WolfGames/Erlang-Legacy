using System.Collections;
using Utils;
using System.Collections.Generic;
using UnityEngine;

public enum LifeBarAction
{
    setUp, gainLife, loseLife, addNewLife, fillAll
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
    Queue<(LifeBarAction, int)> pendentChanges;
    bool modifying;

    //pre: --
    //post: We initialize what we need
    void Awake()
    {
        lifeContainers = new List<LifeContainer>();
        pendentChanges = new Queue<(LifeBarAction, int)>();
    }

    //pre: --
    //post:-- If we have pendent changes, we call the queue manager
    void FixedUpdate()
    {
        if (!modifying && pendentChanges.Count != 0)
        {
            StartCoroutine(ManageQueue());
        }
    }

    #region  public methods

    //pre: initialLifes > 0 && < cMaxLifeContainers
    //post: it puts to the quque the proces that initializes the lifeBar
    public void SetUpLifes(int initialLifes)
    {
        pendentChanges.Clear();
        modifying = false;
        pendentChanges.Enqueue((LifeBarAction.setUp, initialLifes));
    }

    //pre: lifesUp > 0 
    //post: it puts to the quque the proces that gains/heals the lifeBar
    public void GainLifes(int lifesUp)
    {
        pendentChanges.Enqueue((LifeBarAction.gainLife, lifesUp));
    }

    //pre: lifesOut > 0 
    //post: it puts to the quque the proces that loses lifes 
    public void LoseLifes(int lifesOut)
    {
        pendentChanges.Enqueue((LifeBarAction.loseLife, lifesOut));
    }

    //pre: --
    //post: it puts to the quque the proces that heals all lifes 
    public void HealAllLifes()
    {
        pendentChanges.Enqueue((LifeBarAction.gainLife, totalLifes - currentLifes));
    }

    //pre: --
    //post: it puts to the quque the proces puts a new life to barlife
    public void SetUpNewLife()
    {
        pendentChanges.Clear();
        pendentChanges.Enqueue((LifeBarAction.addNewLife, 0));
    }

    #endregion

    #region private methods

    //pre: --
    //post: Manage Queue is the manager of the pendent changes.
    //      its going to inicialize the diferent proces, and assure they are finished before starting others.
    private IEnumerator ManageQueue()
    {
        if (modifying || pendentChanges.Count == 0)
            yield break;

        modifying = true;

        (LifeBarAction, int) actionType = pendentChanges.Dequeue();

        switch (actionType.Item1)
        {
            case LifeBarAction.setUp:
                yield return StartCoroutine(SetUpLifesProcess(actionType.Item2));
                break;
            case LifeBarAction.gainLife:
                yield return StartCoroutine(GainLifesProcess(actionType.Item2));
                break;
            case LifeBarAction.loseLife:
                yield return StartCoroutine(LoseLifesProcess(actionType.Item2));
                break;
            case LifeBarAction.fillAll:
                yield return StartCoroutine(FillAllLifes());
                break;
            case LifeBarAction.addNewLife:
                yield return StartCoroutine(AddNewLife());
                break;
            default:
                yield return null;
                break;
        }

        modifying = currentLifes == 0;
    }

    //pre: lifesIn > 0 && lifesIn <= 9
    //post: delates current life bar and initializes health bar with the number of lifesIn 
    //      all full (of life)
    IEnumerator SetUpLifesProcess(int initialLifes)
    {
        lifeContainers.Clear();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        totalLifes = initialLifes;
        currentLifes = initialLifes;

        for (int i = 0; i < totalLifes; i++)
        {
            var current = Instantiate(lifePrefab, transform);
            lifeContainers.Insert(0, current.GetComponentInChildren<LifeContainer>());
        }

        yield return null;
    }


    //pre: --
    //post: Preces that makes gain lifes.
    //      if lifes where 1 danger effect is desactivated
    IEnumerator GainLifesProcess(int lifesUp)
    {
        if (currentLifes < totalLifes)
        {
            bool desactivateDangerEffect = currentLifes == 1;

            int lifesUpdate = Mathf.Min(totalLifes, currentLifes + lifesUp);

            List<IEnumerator> coroutines = new List<IEnumerator>();
            for (int i = currentLifes; i < lifesUpdate; i++)
            {
                coroutines.Add(lifeContainers[i].Gain());
            }

            if (desactivateDangerEffect)
            {
                ActivateDangerEffect(false);
            }

            yield return Utils.Functions.CoroutineChaining(coroutines.ToArray());
            currentLifes = lifesUpdate;

        }
        else
        {
            yield return null;
        }
    }

    //pre: --
    //post: Process that fully heals lifebar 
    IEnumerator FillAllLifes()
    {
        bool desactivateDangerEffect = currentLifes == 1;

        List<IEnumerator> coroutines = new List<IEnumerator>();
        for (int i = 0; i < totalLifes; i++)
        {
            if (lifeContainers[i].HasLife())
            {
                coroutines.Add(lifeContainers[i].Reflection());
            }
            else
            {
                coroutines.Add(lifeContainers[i].Gain());
            }
        }

        if (desactivateDangerEffect)
        {
            ActivateDangerEffect(false);
        }

        yield return Utils.Functions.CoroutineChaining(coroutines.ToArray());
        currentLifes = totalLifes;
    }

    //pre: --
    //post: Preces that makes lose lifes.
    //      if remaining lifes == 1 triggers danger effect
    //      if remaining lifes == 0 triggers die effect
    IEnumerator LoseLifesProcess(int lifesOut)
    {
        if (currentLifes > 0)
        {
            int lifesUpdate = Mathf.Max(0, currentLifes - lifesOut);

            Coroutine lastOne = null;
            for (int i = currentLifes - 1; i >= lifesUpdate; i--)
            {
                lastOne = StartCoroutine(lifeContainers[i].Lose());
            }
            currentLifes = lifesUpdate;

            if (currentLifes == 1)
            {
                ActivateDangerEffect(true);
            }
            yield return lastOne;

            if (currentLifes == 0)
            {
                ActivateDangerEffect(false); //we make sure that danger effect is not active anymore
                StartCoroutine(DieEffect());
            }
        }
        else
        {
            yield return null;
        }
    }

    //pre: --
    //post: Fill all lifes and only afert lifes are full, calls the method to set up new life
    IEnumerator AddNewLife()
    {
        List<IEnumerator> lst = new List<IEnumerator>();
        lst.Add(FillAllLifes());
        lst.Add(NewLife());
        yield return Utils.Functions.CoroutineChaining(lst.ToArray());
    }

    //pre:  totalLifes > 9, health bar fully filled
    //post: adds NewLife to LifeBar
    private IEnumerator NewLife()
    {
        //we add life and assume that all other lifes are fully filled 
        totalLifes++;
        currentLifes = totalLifes;
        //we instanciate an emptyLifePrefab to the place of the new life
        GameObject current = Instantiate(emptyLifePrefab, transform);
        current.transform.SetAsFirstSibling();
        // we trigger the particle effect to the empty object (so its visible) and we wait it's duration
        ParticleSystem particleEffect = Instantiate(newLifeApearingParticleEffect, current.transform);
        particleEffect.Play();
        yield return new WaitForSeconds(particleEffect.main.duration * 2);
        //Destroy current empty object and instensiate the new life
        Destroy(current);
        GameObject currentLife = Instantiate(lifePrefab, transform);
        currentLife.transform.SetAsFirstSibling();
        lifeContainers.Add(currentLife.GetComponentInChildren<LifeContainer>());
        //Life fills to make nice effect
        StartCoroutine(lifeContainers[totalLifes - 1].Reflection());
    }

    //pre: lifeContainer[0] it's the last remaining life
    //post: Danger effect for last life is activated or desactiveted dependig on bool
    private void ActivateDangerEffect(bool activate)
    {
        StartCoroutine(lifeContainers[0].lastLife(activate));
    }

    //pre: --
    //post: All life shake for a moment before breaking.
    private IEnumerator DieEffect()
    {
        foreach (LifeContainer life in lifeContainers)
        {
            life.SetShake(true);
        }
        yield return new WaitForSeconds(cCoroutineShakeWait);
        foreach (LifeContainer life in lifeContainers)
        {
            life.BrokeLife();
            life.SetShake(false);
        }
    }

    #endregion

}
