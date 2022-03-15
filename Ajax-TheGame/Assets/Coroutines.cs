using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Functions = Utils.Functions;

public class Coroutines : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // SECUENTIAL COROUTINES (uncomment example code bellow)
        // var chaining = Functions.CoroutineChaining(SayHello(), BeforeHello());
        // StartCoroutine(chaining);

        // ASYNC COROUTINE EXECUTION (uncomment code bellow)    
        // StartCoroutine(SayHello());
        // StartCoroutine(BeforeHello());
    }


    IEnumerator SayHello()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("SayHello");
    }

    IEnumerator BeforeHello()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("If i am not in a chainnning I should be before hello");
    }
}
