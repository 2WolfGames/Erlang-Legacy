using Core.Shared;
using Core.Shared.Enum;
using UnityEngine;
using UnityEngine.UI;
using Core.Player.Controller;
using DG.Tweening;

namespace Core.Cinematographics
{
    public class C_EndDemo_controller : MonoBehaviour
    {
        [SerializeField] GameObject kassandraBody;
        [SerializeField] Animator kassandraAnimator;
        [SerializeField] Transform startAnimationPoint;
        [SerializeField] Image blackEndImage;
        bool transitionStarted = false;

        //pre: --
        //post: on player colision starts end demo cinematographic 
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                StartEndDemoTransition();
            }
        }

        //pre: --
        //post: chacks if there is no transition started, 
        //      if correct Ajax goes to start animation point and starts KassyRunsIntoAjaxArms().
        public void StartEndDemoTransition()
        {
            if (transitionStarted)
                return;

            transitionStarted = true;

            PlayerController.Instance.Controllable = false;

            MovePlayer.Trigger(startAnimationPoint, 0f, Face.Left, 1f, () =>
            {
                KassyRunsIntoAjaxArms();
            });
        }

        //pre: --
        //post: Unity Event for the end of Captain Java speach: loads new scene.
        void KassyRunsIntoAjaxArms()
        {
            kassandraBody.transform.localScale= new Vector3(-1,1,1);
            kassandraBody.transform.position = new Vector3(34.44f, 0,0);
            kassandraBody.transform.DOLocalMoveX(39f,2f).SetDelay(2f).OnStart(() => {
                kassandraAnimator.SetTrigger("run");
            }).OnComplete(() => {
                kassandraAnimator.SetTrigger("hug");
                EndGame();
            });
        }

        //`pre:--
        //post: Show game credits after fade screen away
        void EndGame(){
            blackEndImage.DOFade(1,5f).OnComplete(() => {
                StartCoroutine(Loader.LoadWithDelay(SceneID.EndGameScene, 1f));
            });
        }
    }
}