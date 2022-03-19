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

    void Start()
    {
        currentLifes = ajax.Life;
        lifeBar.SetUpLifes(currentLifes);
    }

    void FixedUpdate()
    {
        if (ajax.Life != currentLifes)
        {
            int x = Mathf.Abs(currentLifes - ajax.Life);
            if (currentLifes < ajax.Life)
            {
                lifeBar.GainLifes(x);
            }
            else
            {
                lifeBar.LoseLifes(x);
            }
            currentLifes = ajax.Life;
        }
    }
}
