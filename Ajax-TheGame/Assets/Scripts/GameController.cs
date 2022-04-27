using Core.Shared;
using Core.UI;
using Core.UI.LifeBar;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] LifeController playerLifeController;
    [SerializeField] UIPowerTimer dash;
    [SerializeField] UIPowerTimer ray;
    [SerializeField] LifeBarController lifeBar;

    int currentLifes;

    void Start()
    {
        if (playerLifeController)
            currentLifes = playerLifeController.Life;
        lifeBar.SetUpLifes(currentLifes);
    }

    void FixedUpdate()
    {
        if (!playerLifeController)
            return;

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
