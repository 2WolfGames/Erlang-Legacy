using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Shared;
using Core.UI;
using Core.UI.LifeBar;

public class GameController : MonoBehaviour
{
    [SerializeField] LifeController playerLifeController;
    [SerializeField] UIPowerTimer dash;
    [SerializeField] UIPowerTimer ray;
    [SerializeField] LifeBarController lifeBar;

    int currentLifes;

    void Start()
    {
        currentLifes = playerLifeController.Life;
        lifeBar.SetUpLifes(currentLifes);
    }

    void FixedUpdate()
    {
        if (playerLifeController.Life != currentLifes)
        {
            int x = Mathf.Abs(currentLifes - playerLifeController.Life);
            if (currentLifes < playerLifeController.Life)
            {
                lifeBar.GainLifes(x);
            }
            else
            {
                lifeBar.LoseLifes(x);
            }
            currentLifes = playerLifeController.Life;
        }
    }
}
