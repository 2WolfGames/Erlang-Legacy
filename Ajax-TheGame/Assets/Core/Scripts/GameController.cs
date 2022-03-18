using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [SerializeField] LifeController ajax;
    [SerializeField] UIPowerTimer dash;
    [SerializeField] UIPowerTimer ray;
    [SerializeField] LifeBar lifeBar;

    int currentLifes;

    void Start() {
        currentLifes = (int)(ajax.Life/100);
        lifeBar.SetUpLifes((int)(ajax.Life/100)); 
    }

    void FixedUpdate(){
        int updatelifes = (int)(ajax.Life/100);
        if (updatelifes != currentLifes){
            int x = Mathf.Abs(currentLifes-updatelifes);
            if (currentLifes < updatelifes){
                lifeBar.GainLifes(x);
            } else{
                lifeBar.LoseLifes(x);
            }
            currentLifes = updatelifes;
        }
    }
}
