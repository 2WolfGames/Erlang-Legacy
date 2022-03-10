using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject lifePrefab;
    [SerializeField] List<LifeContainer> lifeContainers;
    [SerializeField] ParticleSystem newLifeApearingParticleEffect;
    int totalLifes;
    int currentLifes;

    // Start is called before the first frame update
    void Start()
    {
        lifeContainers = new List<LifeContainer>();
    }

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

    public void AddLifes(int lifesUp){
        if (currentLifes < totalLifes){
            bool desactivateDangerEffect = currentLifes == 1;

            for (int i = currentLifes; i < Mathf.Min(totalLifes,currentLifes+lifesUp) ; i++){
                lifeContainers[i].Add();
            }

            currentLifes = Mathf.Min(totalLifes,currentLifes+lifesUp);

            if(desactivateDangerEffect){
                ActivateDangerEffect(false);
            } else if (currentLifes == totalLifes){
                StartCoroutine(AllLifesEffect());
            }
        }
    }

    public void RemoveLife(int lifesOut){
        if(currentLifes > 0){
            for (int i = currentLifes - 1; i >= Mathf.Max(currentLifes - lifesOut,0) ; i--){
                lifeContainers[i].Remove();
            }
            currentLifes = Mathf.Max(0,currentLifes-lifesOut);

            if(currentLifes == 1){
                ActivateDangerEffect(true);
            } else if (currentLifes == 0){
                ActivateDangerEffect(false);
                StartCoroutine(DieEffect(0));
            }
        }
    }

    public void SetUpOneMoreLife(){
        if (totalLifes < 9){
            StartCoroutine(FillAllLifesAndAddOne(0));
        }
    }

    private IEnumerator FillAllLifesAndAddOne(int i) {
        if (!lifeContainers[i].HasLife()){
            lifeContainers[i].Add();
            yield return new WaitForSeconds(1f);
        } else {
            lifeContainers[i].Reflection();
            yield return new WaitForSeconds(0.35f);
        }
        if (i < totalLifes - 1){
            StartCoroutine(FillAllLifesAndAddOne(++i));
        } else {
            StartCoroutine(NewLife());
        }
    }

    private IEnumerator NewLife(){
        totalLifes++;
        currentLifes = totalLifes;

        GameObject currentLife = Instantiate(lifePrefab,transform);
        currentLife.GetComponentInChildren<LifeContainer>().HideAll(true);
        currentLife.GetComponent<Image>().enabled = false;
        currentLife.transform.SetAsFirstSibling();
        
        ParticleSystem particleEffect = Instantiate(newLifeApearingParticleEffect, currentLife.transform);
        
        particleEffect.Play();
        yield return new WaitForSeconds(particleEffect.main.duration*2);

        lifeContainers.Add(currentLife.GetComponentInChildren<LifeContainer>());  
        currentLife.GetComponent<Image>().enabled = true;
        lifeContainers[totalLifes - 1].HideAll(false);        
    }

    private void ActivateDangerEffect(bool activate){
        lifeContainers[0].GetComponentInChildren<LifeContainer>().lastLife(activate);
    }

    private IEnumerator DieEffect(int i){
        foreach( LifeContainer life in lifeContainers ){
            life.SetShake(true);
        }
        yield return new WaitForSeconds(1.25f);
        foreach( LifeContainer life in lifeContainers ){
            life.BrokeLife();
            life.SetShake(false);
        }
    }

    private IEnumerator AllLifesEffect(){
        foreach( LifeContainer life in lifeContainers ){
            life.SetShake(true);
        }
        yield return new WaitForSeconds(1);
        foreach( LifeContainer life in lifeContainers ){
            life.SetShake(false);
        }
    }


}
