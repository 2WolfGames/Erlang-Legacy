using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSessionController : MonoBehaviour
{
    Core.UI.LifeBar.LifeBarController lifeBarController;
    /*Save Point*/
    Vector3 savePoint;
    string sceneSavePoint;
    /*Current Point*/
    Vector3 currentPoint;
    bool setUpLifes = false;

    /*for test delete later*/
    [SerializeField] bool loseLife = false;
    int lifes = 5;
    /*for test delete later*/

    void Awake(){
        int numGameSessionControllers = FindObjectsOfType<GameSessionController>().Length;
        if (numGameSessionControllers > 1){
            Destroy(gameObject);
        } else{
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start() {
        lifes = 5; //Delete
        FindLifebar();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindLifebar();
    }

    // Update is called once per frame
    void Update()
    {
        if (setUpLifes){
            lifeBarController?.SetUpLifes(lifes,5); //Todo
            setUpLifes = false;
        }

        if (loseLife){
            lifes--;
            lifeBarController.LoseLifes(1);
            loseLife = false;
            if (lifes == 0){
                ResetGameToSavePoint();
            }
        }
    }

    public void SavePlayerCurrentPoint(Transform currentPointTransform){
        currentPoint = currentPointTransform.position;
    }

    public void SavePlayerSavePoint(Transform savePointTransform, string savePointScene){
        savePoint = savePointTransform.position;
        sceneSavePoint = savePointScene;
    }

    public void ResetGameToSavePoint(){
        lifes = 5;
        FindObjectOfType<InGameCanvas>()?.ActiveDeathImage();
        StartCoroutine(Core.Shared.Loader.LoadWithDelay(sceneSavePoint,6));
    }

    public bool IsCurrentSavePoint(){
        return SceneManager.GetActiveScene().name == sceneSavePoint && savePoint != null;
    }

    public Vector3 GetSavePoint(){
        return savePoint;
    }

    public Vector3 GetCurrentPoint(){
        return currentPoint;
    }

    private void FindLifebar(){
        lifeBarController = FindObjectOfType<Core.UI.LifeBar.LifeBarController>();
        setUpLifes = true;
    }
}
