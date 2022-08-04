using Core.Shared.Enum;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Core.Shared;
using UnityEngine.UI;

public class EndGameSceneBehaviour : MonoBehaviour
{
    [SerializeField] GameObject ladybug;
    [SerializeField] TextMeshProUGUI createdBy;
    [SerializeField] GameObject creators;
    [SerializeField] Image gameTitle;

    // Start is called before the first frame update
    void Start()
    {
        ShowTitle();
    }

    private void ShowTitle()
    {
        gameTitle.DOFade(1, 5f).SetDelay(2f).OnComplete(() =>
        {
            ShowCredits();
        });
    }

    private void ShowCredits()
    {
        createdBy.rectTransform.DOLocalMoveY(220, 3f).OnComplete(() =>
        {
            creators.transform.DOLocalMoveY(15,3f);
            MoveBugs();
        });
    }

    private void MoveBugs(){
        ladybug.transform.DOLocalMoveX(600,10f).SetDelay(6f).OnComplete(() => {
            FadeAllAndGoToStart();
        });
    }

    private void FadeAllAndGoToStart(){
        GetComponent<CanvasGroup>().DOFade(0,10f).SetDelay(5f).OnComplete(() => {
            StartCoroutine(Loader.LoadWithDelay(SceneID.StartMenu, 0));
        });
    }


}
