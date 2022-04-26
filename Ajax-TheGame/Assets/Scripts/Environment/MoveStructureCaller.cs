using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStructureCaller : MonoBehaviour
{
    [SerializeField] MoveStructure moveStructure;

    //pre: --
    //post: Is collider is player, it calls the method Move of moveStructure. 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            moveStructure.Move(); //Activation of Mecanism 
        }
    }

}
