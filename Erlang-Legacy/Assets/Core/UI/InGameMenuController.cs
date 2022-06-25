using System.Collections;
using Core.Player.Controller;
using Core.Shared;
using Core.Shared.Enum;
using Spine.Unity;
using UnityEngine;

namespace Core.UI
{
    public class InGameMenuController : MonoBehaviour
    {
        public static bool gamePaused = false;
        [SerializeField] GameObject pauseMenu;
        [SerializeField] GameObject settingsMenu;
        [SerializeField] GameObject AjaxDiaryPrefab;
        SkeletonGraphic skeletonGraphic;
        GameObject currentAjaxDiary;
        //int diaryPage; for the moment we only have one page
        int option;
        bool inSettingsPage = false;

        private void Update()
        {

            if (gamePaused && !inSettingsPage)
            {
                ManageOptionsPage();
            }

            if (Input.GetKeyDown(KeyCode.Tab) && !inSettingsPage)
            {
                if (gamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }

        }

        private void ResumeGame()
        {
            CloseMenu();
            Time.timeScale = 1;
            gamePaused = false;
            StartCoroutine(EnablePlayer());
            pauseMenu.SetActive(false);
        }

        //Is necessary to wait till the end of frame for not affect the player movment with the key events of the menu.
        IEnumerator EnablePlayer()
        {
            yield return new WaitForEndOfFrame();

            var player = PlayerController.Instance;
            player.BlockingUI = false;
        }

        private void PauseGame()
        {
            pauseMenu.SetActive(true);
            OpenMenu();

            var player = PlayerController.Instance;
            player.BlockingUI = true;

            Time.timeScale = 0; //we stop game 
            gamePaused = true;
        }

        #region Menu

        private void ManageOptionsPage()
        {
            if (option == 0)
            { //Resume
                if (Input.GetKeyDown(KeyCode.S))
                {
                    OnResumeHoverOut();
                    OnSettingsHoverIn();
                    option = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    ResumeGame();
                }
            }
            else if (option == 1)
            { // Settings
                if (Input.GetKeyDown(KeyCode.S))
                {
                    OnSettingsHoverOut();
                    OnQuitHoverIn();
                    option = 2;
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    OnSettingsHoverOut();
                    OnResumeHoverIn();
                    option = 0;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Scene Manager
                    OpenSettingsPage();
                }
            }
            else
            { //Quit
                if (Input.GetKeyDown(KeyCode.W))
                {
                    OnQuitHoverOut();
                    OnSettingsHoverIn();
                    option = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Time.timeScale = 1;
                    StartCoroutine(Loader.LoadWithDelay(SceneID.StartMenu, 0));
                }
            }
        }

        private void OpenMenu()
        {
            //diaryPage = 1;
            option = 0;
            currentAjaxDiary = Instantiate(AjaxDiaryPrefab, pauseMenu.transform);
            skeletonGraphic = currentAjaxDiary.GetComponent<SkeletonGraphic>();
            skeletonGraphic.AnimationState.SetAnimation(1, "init", false);
        }

        private void CloseMenu()
        {
            Destroy(currentAjaxDiary);
        }

        private void OpenSettingsPage(){
            inSettingsPage = true;
            settingsMenu.SetActive(true);
            settingsMenu.GetComponent<SettingsMenu>()?.OnOpenMenu(true);
        }

        public void CloseSettingsPage(){
            settingsMenu.SetActive(false);
            inSettingsPage = false;
        }


        #region menu animations

        public void OnResumeHoverIn()
        {
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverInResume", false, 0);
        }

        public void OnResumeHoverOut()
        {
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverOutResume", false, 0);
        }

        public void OnSettingsHoverIn()
        {
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverInSettings", false, 0);
        }

        public void OnSettingsHoverOut()
        {
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverOutSettings", false, 0);
        }

        public void OnQuitHoverIn()
        {
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverInQuit", false, 0);
        }

        public void OnQuitHoverOut()
        {
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverOutQuit", false, 0);
        }

        #endregion

        #endregion

    }
}
