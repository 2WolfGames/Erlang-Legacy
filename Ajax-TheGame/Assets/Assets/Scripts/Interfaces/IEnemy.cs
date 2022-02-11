using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: think about a better name for this interface: name can be more generic because ajax himself can implement it
public interface IEnemy
{
    /**
        thought to return true when ever the
        character was hit and die because of hit
    */
    bool OnHit(float damage);

    void OnDie();

}
