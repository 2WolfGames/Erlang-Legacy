using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Core.Shared;
using Core.Player.Controller;
using Core.Shared.Enum;

namespace Core.Cinematographics
{
    public class C_IntroP1_controller : MonoBehaviour
    {
        [SerializeField] CanvasGroup initIndicationsGroup;
        [SerializeField] CanvasGroup initIndications;
        [SerializeField] Image cloud1;
        [SerializeField] Image cloud2;
        [SerializeField] Image gameTitle;
        [SerializeField] Animator pigAnimator;


        //pre: --
        //post: we setup everythig for the cinematographic and start showin the keybard indications
        void Start()
        {
            PlayerController.Instance.Controllable = false;
            initIndications.DOFade(1, 2f).SetDelay(2f).OnComplete(() =>
            {
                FadeIndications();
            });
        }

        //pre: --
        //post: Indications fade away and we show the tiltle
        void FadeIndications()
        {
            initIndicationsGroup.DOFade(0, 3f).SetDelay(5f).OnComplete(() =>
            {
                ShowTitle();
            });
        }

        //pre: --
        //post: Clouds go away and show game title
        void ShowTitle()
        {
            cloud2.rectTransform.DOLocalMoveX(-1000, 5f);
            cloud1.rectTransform.DOLocalMoveX(1000, 7f).SetDelay(1f);
            gameTitle.rectTransform.DOScale(new Vector3(1.25f, 1.25f, 1), 3f).OnComplete(() =>
            {
                HideTitle();
            });
        }

        //pre: --
        //post: Game title fades out
        void HideTitle()
        {
            gameTitle.DOFade(0, 3f).SetDelay(5f).OnComplete(() =>
            {
                AjaxIsKicked();
            });
        }

        //pre: --
        //post: Apache pig apears and thows ajax down the clift
        void AjaxIsKicked()
        {
            pigAnimator.transform.DOLocalMoveX(81, 3f).SetDelay(3f).OnStart(() =>
            {
                //HERE scream 
                PlayerController.Instance.SetFacing(Face.Right);
                //HERE starts battle pig music
                pigAnimator.SetBool("Run", true);
            }).OnComplete(() =>
            {
                pigAnimator.SetBool("Run", false);
                pigAnimator.transform.localScale = new Vector3(1, 1, 1);
                pigAnimator.SetTrigger("Coz");
                pigAnimator.transform.DOLocalMoveX(81, 1f).OnComplete(() =>
                {
                    AjaxFalls();
                });
            });
        }

        //pre: --
        //post: Ajax falls and starts second scene cinematoraphic intro
        void AjaxFalls()
        {
            PlayerController.Instance.Animator.SetTrigger("jump");
            PlayerController.Instance.Animator.SetBool("jumping", true);
            PlayerController.Instance.transform.DOLocalJump(new Vector3(60, 3, 1), 20f, 1, 5f).OnComplete(() =>
            {
                StartCoroutine(Loader.LoadWithDelay(SceneID.Cinematographic_Intro_P2, 0.1f));
            });
        }

    }
}