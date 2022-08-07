using Core.Shared.Enum;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Core.Shared;
using UnityEngine.UI;

namespace Core.Cinematographics
{
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

        //pre: --
        //post: Shows  game title and calls ShowCredits()
        private void ShowTitle()
        {
            gameTitle.DOFade(1, 5f).SetDelay(2f).OnComplete(() =>
            {
                ShowCredits();
            });
        }

        //pre: --
        //post: Shows credits and calls MoveBugs()
        private void ShowCredits()
        {
            createdBy.rectTransform.DOLocalMoveY(200, 3f).OnComplete(() =>
            {
                creators.transform.DOLocalMoveY(15, 3f);
                MoveBugs();
            });
        }

        //pre: --
        //post: Makes ladybug move through scene and starts fading all away
        private void MoveBugs()
        {
            ladybug.transform.DOLocalMoveX(600, 10f).SetDelay(6f).OnComplete(() =>
            {
                FadeAllAndGoToStart();
            });
        }

        //pre: --
        //post: Fades all away and returns to start menu
        private void FadeAllAndGoToStart()
        {
            GetComponent<CanvasGroup>().DOFade(0, 10f).SetDelay(5f).OnComplete(() =>
            {
                StartCoroutine(Loader.LoadWithDelay(SceneID.StartMenu, 0));
            });
        }

    }
}