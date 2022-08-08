using Core.GameSession;
using Core.Manager;
using Core.ScriptableEffect;
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
        [SerializeField] GameObject selectGameMenu;
        [SerializeField] GameObject selectGameMenuSelector;
        [SerializeField] Transform optionCurrentGame;
        [SerializeField] Transform optionNewGame;
        [SerializeField] Image worldImage;
        [SerializeField] Image cloudsImage;

        [Header("Sound clips")]
        [SerializeField] AudioClip navigationSound;
        [SerializeField] AudioClip selectSound;
        [SerializeField] VolumeSettings playerVolumeSettings;

        private int option;
        private bool inSettingsPage = false;
        private bool inSelectGameMenu = false;
        private int optionSelectGameMenu = 0;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        //pre: --
        //post: all is set to initial state
        private void Start()
        {
            skeletonGraphic.AnimationState.SetAnimation(1, "init", false);
            skeletonGraphic.AnimationState.AddAnimation(1, "hoverInStart", false, 0);
            MoveSelectGameSelector();
            option = 0;
        }

        //pre: --
        //post: controls user interactions with menu
        private void Update()
        {
            if (inSettingsPage)
                return;

            ManageSelectGameMenu();
            ManageOptions();
        }

        //pre:--
        //post: animates objects in scene
        private void FixedUpdate()
        {
            Function.RotateGameObject(worldImage.transform, -20);
            Function.RotateGameObject(cloudsImage.transform, 30);
        }

        //pre: --
        //post: controls user interactions 
        private void ManageOptions()
        {
            if (option == 0)
            { //Resume
                if (Input.GetKeyDown(KeyCode.S))
                {
                    PlayNavigationSound();
                    OnStartGameHoverOut();
                    OnSettingsHoverIn();
                    HideSelectGameMenu();
                    option = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    PlaySelectSound();
                    LoadGame();
                }
            }
            else if (option == 1)
            { // Settings
                if (Input.GetKeyDown(KeyCode.S))
                {
                    PlayNavigationSound();
                    OnSettingsHoverOut();
                    OnQuitHoverIn();
                    option = 2;
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    PlayNavigationSound();
                    OnSettingsHoverOut();
                    OnStartGameHoverIn();
                    option = 0;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Scene Manager
                    PlaySelectSound();
                    OpenSettingsPage();
                }
            }
            else
            { //Quit
                if (Input.GetKeyDown(KeyCode.W))
                {
                    PlayNavigationSound();
                    OnQuitHoverOut();
                    OnSettingsHoverIn();
                    option = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    PlaySelectSound();
                    Application.Quit();
                }
            }
        }

        private void ManageSelectGameMenu()
        {
            if (!inSelectGameMenu)
                return;

            if (optionSelectGameMenu == 0)
            { //current game
                if (Input.GetKeyDown(KeyCode.D))
                {
                    optionSelectGameMenu = 1;
                    PlayNavigationSound();
                    MoveSelectGameSelector();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    //PlaySelectSound();
                    LoadCurrentGame();
                }
            }
            else
            { //op == 1 //new game 
                if (Input.GetKeyDown(KeyCode.A))
                {
                    optionSelectGameMenu = 0;
                    PlayNavigationSound();
                    MoveSelectGameSelector();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    //PlaySelectSound();
                    LoadNewGame();
                }
            }

        }

        private void PlaySelectSound()
        {
            SoundManager soundManager = SoundManager.Instance;
            float volume = playerVolumeSettings ? playerVolumeSettings.SoundVolume : 1;
            soundManager?.PlaySound(selectSound, volume, audioSource);

            if (soundManager == null) Debug.LogWarning("SoundManager is null");
        }

        private void PlayNavigationSound()
        {
            SoundManager soundManager = SoundManager.Instance;
            float volume = playerVolumeSettings ? playerVolumeSettings.SoundVolume : 1;
            soundManager?.PlaySound(navigationSound, volume, audioSource);

            if (soundManager == null) Debug.LogWarning("SoundManager is null");
        }


        //pre: --
        //post: laods game
        private void LoadGame()
        {
            if (!SaveSystem.SaveGameExists())
            {
                LoadNewGame();
            }
            else
            {
                OpenSelectGameMenu();
            }

        }

        private void LoadNewGame()
        {
            //Crear inicial file
            SaveSystem.InitializeGame();
            PlayerState playerState = SaveSystem.LoadPlayerState();
            GameSessionController.Instance.LoadData = true;
            StartCoroutine(Loader.LoadWithDelay((SceneID)playerState.scene, 0));
        }

        private void LoadCurrentGame()
        {
            PlayerState playerState = SaveSystem.LoadPlayerState();
            GameSessionController.Instance.LoadData = true;
            StartCoroutine(Loader.LoadWithDelay((SceneID)playerState.scene, 0));
        }

        //pre: --
        //post: opens settings menu 
        private void OpenSettingsPage()
        {
            inSettingsPage = true;
            settingsMenu.SetActive(true);
            settingsMenu.GetComponent<SettingsMenu>()?.OnOpenMenu(false);
        }

        //pre: --
        //post: closes settings menu 
        public void CloseSettingsPage()
        {
            settingsMenu.SetActive(false);
            inSettingsPage = false;
        }

        void OpenSelectGameMenu()
        {
            if (inSelectGameMenu)
                return;

            selectGameMenu.GetComponent<CanvasGroup>().alpha = 1;
            inSelectGameMenu = true;
        }

        void HideSelectGameMenu()
        {
            selectGameMenu.GetComponent<CanvasGroup>().alpha = 0;
            inSelectGameMenu = false;
        }

        //Animations interactions with menu 
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

        private void MoveSelectGameSelector()
        {
            if (optionSelectGameMenu == 0)
            {
                selectGameMenuSelector.transform.position = new Vector3(
                    optionCurrentGame.transform.position.x,
                    selectGameMenuSelector.transform.position.y,
                    selectGameMenuSelector.transform.position.z);
            }
            else
            {
                selectGameMenuSelector.transform.position = new Vector3(
                    optionNewGame.transform.position.x,
                    selectGameMenuSelector.transform.position.y,
                    selectGameMenuSelector.transform.position.z);
            }
        }

        #endregion
    }
}
