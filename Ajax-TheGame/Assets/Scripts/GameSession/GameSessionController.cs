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
    Core.Shared.Loader.Entrance entranceTag;
    bool searchCurrentPoint = false;
    bool setUpLifes = false;
    bool hasDied = false;

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
        if (searchCurrentPoint){
            foreach (SceneEntrance se in FindObjectsOfType<SceneEntrance>()) {
                if (se.gameObject.CompareTag(entranceTag.ToString())){
                    currentPoint = se.GetEntrancePoint();
                    searchCurrentPoint = false;
                    break;
                }
            }
        }
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
        hasDied = true;
        FindObjectOfType<InGameCanvas>()?.ActiveDeathImage();
        StartCoroutine(Core.Shared.Loader.LoadWithDelay(sceneSavePoint,6));
    }

    public Vector3 GetCurrentPoint(){
        if (hasDied && SceneManager.GetActiveScene().name == sceneSavePoint && savePoint != null){
            hasDied = false;
            return savePoint;
        } else{
            return currentPoint;
        }
    }

    public void SearchCurrentPoint(Core.Shared.Loader.Entrance entranceTag){
        searchCurrentPoint = true;
        this.entranceTag = entranceTag;
    }

    private void FindLifebar(){
        lifeBarController = FindObjectOfType<Core.UI.LifeBar.LifeBarController>();
        setUpLifes = true;
    }
}
