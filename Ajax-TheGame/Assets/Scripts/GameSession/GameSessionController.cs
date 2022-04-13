using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameSessionController : MonoBehaviour
{
    Core.UI.LifeBar.LifeBarController lifeBarController;
    [SerializeField] Image deathImage;

    /*for test delete later*/
    [SerializeField] bool loseLife = false;
    int lifes = 5;
    /*for test delete later*/
    
    /*Save Point*/
    Vector3 savePoint;
    string sceneSavePoint;
    /*Current Point*/
    Vector3 currentPoint;

    void Awake(){
        int numGameSessionControllers = FindObjectsOfType<GameSessionController>().Length;
        if (numGameSessionControllers > 1){
            Destroy(gameObject);
        } else{
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start() {
        lifes = 5;
        lifeBarController = FindObjectOfType<Core.UI.LifeBar.LifeBarController>();
        lifeBarController.SetUpLifes(lifes); //TO DO: This has to check the scriptabble object of life
    }

    // Update is called once per frame
    void Update()
    {
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
        deathImage.DOFade(1,3).SetDelay(2).OnComplete( () =>
        {
            Core.Shared.Loader.Load(sceneSavePoint);
        });
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

}
