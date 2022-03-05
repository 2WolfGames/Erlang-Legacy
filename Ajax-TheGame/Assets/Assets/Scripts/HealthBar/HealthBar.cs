using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject lifeContainerPrefab;
    [SerializeField] List<GameObject> lifeContainers;
    int totalLifes;
    int currentLifes;

    // Start is called before the first frame update
    void Start()
    {
        lifeContainers = new List<GameObject>();
    }

    public void SetUpLifes(int lifesIn){
        lifeContainers.Clear();
        for(int i = transform.childCount - 1 ; i >= 0; i-- ){
            Destroy(transform.GetChild(i).gameObject);
        }

        totalLifes = lifesIn;
        currentLifes = lifesIn;

        for(int i = 0; i < totalLifes; i++){
            var current = Instantiate(lifeContainerPrefab,transform);
            lifeContainers.Add(current);
        }
    }

    public void AddLifes(int lifesUp){
        bool desactivateDangerEffect = currentLifes == 1;

        for (int i = currentLifes; i < Mathf.Min(totalLifes,currentLifes+lifesUp) ; i++){
            lifeContainers[i].GetComponentInChildren<LifeContainer>().Add();
        }

        currentLifes = Mathf.Min(totalLifes,currentLifes+lifesUp);

        if(desactivateDangerEffect){
            ActivateDangerEffect(false);
        } else if (currentLifes == totalLifes){
            StartCoroutine(AllLifesEffect());
        }
    }

    public void RemoveLife(int lifesOut){
        for (int i = currentLifes - 1; i >= Mathf.Max(currentLifes - lifesOut,0) ; i--){
            if (i != 0){
                lifeContainers[i].GetComponentInChildren<LifeContainer>().Remove();
            }
        }
        currentLifes = Mathf.Max(0,currentLifes-lifesOut);

        if(currentLifes == 1){
            ActivateDangerEffect(true);
        } else if (currentLifes == 0){
            StartCoroutine(DieEffect(0));
        }
    }

    public void SetUpOneMoreLife(){
        if (totalLifes < 9){
            
            AddLifes(totalLifes-currentLifes);

            totalLifes++;
            currentLifes = totalLifes;

            var current = Instantiate(lifeContainerPrefab,transform);
            lifeContainers.Add(current);   
        }
    }

    private void ActivateDangerEffect(bool activate){
        lifeContainers[0].GetComponentInChildren<LifeContainer>().SetShake(activate);
    }

    private IEnumerator DieEffect(int i){
        lifeContainers[i].GetComponentInChildren<LifeContainer>().BrokeLife();
        yield return new WaitForSeconds(0.5f);
        if (i < totalLifes - 1){
            StartCoroutine(DieEffect(++i));
        }
    }

    private IEnumerator AllLifesEffect(){
        foreach( GameObject life in lifeContainers ){
            life.GetComponentInChildren<LifeContainer>().SetShake(true);
        }
        yield return new WaitForSeconds(1);
        foreach( GameObject life in lifeContainers ){
            life.GetComponentInChildren<LifeContainer>().SetShake(false);
        }
    }


}
