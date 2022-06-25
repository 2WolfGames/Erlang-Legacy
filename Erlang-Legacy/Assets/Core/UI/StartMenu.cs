using Core.GameSession;
using Core.Shared;
using Core.Shared.Enum;
using Core.Shared.SaveSystem;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] SkeletonGraphic skeletonGraphic;
        [SerializeField] GameObject settingsMenu;
        [SerializeField] Image worldImage;
        [SerializeField] Image cloudsImage;
        int option;

        bool inSettingsPage = false;

        private void Start()
        {
            skeletonGraphic.AnimationState.SetAnimation(1, "init", false);
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverInStart", false, 0);
            option = 0;
        }

        private void Update()
        {
            if (!inSettingsPage)
            {
                ManageOptions();
            }
        }

        private void FixedUpdate()
        {
            Function.RotateGameObject(worldImage.transform, -20);
            Function.RotateGameObject(cloudsImage.transform, 30);
        }

        private void ManageOptions()
        {
            if (option == 0)
            { //Resume
                if (Input.GetKeyDown(KeyCode.S))
                {
                    OnStartGameHoverOut();
                    OnSettingsHoverIn();
                    option = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    LoadGame();
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
                    OnStartGameHoverIn();
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
                    Application.Quit();
                }
            }
        }

        private void LoadGame()
        {
            if (!SaveSystem.SaveGameExists())
            {
                //Crear inicial file
                SaveSystem.InitializeGame();
            }
            PlayerState playerState = SaveSystem.LoadPlayerState();
            GameSessionController.Instance.LoadData = true;
            StartCoroutine(Loader.LoadWithDelay((SceneID)playerState.scene, 0));
        }

        private void OpenSettingsPage()
        {
            inSettingsPage = true;
            settingsMenu.SetActive(true);
            settingsMenu.GetComponent<SettingsMenu>()?.OnOpenMenu(false);
        }

        public void CloseSettingsPage()
        {
            settingsMenu.SetActive(false);
            inSettingsPage = false;
        }

        #region start menu animations
        public void OnStartGameHoverIn()
        {
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverInStart", false, 0);
        }

        public void OnStartGameHoverOut()
        {
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverOutStart", false, 0);
        }

        public void OnSettingsHoverIn()
        {
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverInOptions", false, 0);
        }

        public void OnSettingsHoverOut()
        {
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverOutOptions", false, 0);
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
    }
}
