using UnityEngine;
using Core.Player;
using Core.Player.Controller;

namespace Core.UI
{
    public class ManagePowersVisibility : MonoBehaviour
    {
        [SerializeField] GameObject dashPower;
        [SerializeField] GameObject rayPower;


        //pre: --
        //post: hides Game Objects depending if player has the abilites or not
        public void ManageAdquiredPowersVisibility()
        {
            rayPower.SetActive(PlayerController.Instance.AdquiredAbility(Ability.Ray));
            dashPower.SetActive(PlayerController.Instance.AdquiredAbility(Ability.Dash));
        }

        //pre: --
        //post: shows everything 
        public void ShowAllPowers()
        {
            rayPower.SetActive(true);
            dashPower.SetActive(true);
        }
    }
}