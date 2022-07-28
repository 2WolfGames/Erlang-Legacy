using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Core.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        const float slidersChangeValue = 0.1f;
        [SerializeField] CanvasGroup optionsCanvasGroup;
        [SerializeField] CanvasGroup keyCheatSheetCanvasGroup;
        [SerializeField] GameObject musicGO;
        [SerializeField] GameObject soundGO;
        [SerializeField] GameObject keysCheatSheetGO;
        [SerializeField] GameObject quitGO;
        [SerializeField] GameObject selector;

        [Header("Sound clips")]
        [SerializeField] AudioClip navigationSound;
        [SerializeField] AudioClip selectSound;
        [SerializeField] AudioClip settingSound;

        private int option = 0;
        private bool showingkeyCheatSheet = false;
        private bool inGame;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        //pre: --
        //post: controls user interactions 
        private void Update()
        {
            if (showingkeyCheatSheet)
            {
                if (Input.anyKeyDown)
                {
                    HideKeySheet();
                    return;
                }
            }

            if (optionsCanvasGroup.alpha != 1)
            {
                return;
            }

            if (option == 0)
            { //Music volume
                if (Input.GetKeyDown(KeyCode.S))
                {
                    option = 1;
                    MoveSelector(soundGO.transform.position.y);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    ManageSlider(musicGO.GetComponentInChildren<Slider>(), false);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    ManageSlider(musicGO.GetComponentInChildren<Slider>(), true);
                }
            }
            else if (option == 1)
            { //VFX Volume
                if (Input.GetKeyDown(KeyCode.S))
                {
                    option = 2;
                    MoveSelector(keysCheatSheetGO.transform.position.y);
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    option = 0;
                    MoveSelector(musicGO.transform.position.y);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    ManageSlider(soundGO.GetComponentInChildren<Slider>(), false);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    ManageSlider(soundGO.GetComponentInChildren<Slider>(), true);
                }
            }
            else if (option == 2)
            { //Button Keysheet
                if (Input.GetKeyDown(KeyCode.S))
                {
                    option = 3;
                    MoveSelector(quitGO.transform.position.y);
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    option = 1;
                    MoveSelector(soundGO.transform.position.y);
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    keysCheatSheetGO.GetComponent<Button>().onClick.Invoke();
                    ShowKeySheet();
                }
            }
            else
            {
                //Quit
                if (Input.GetKeyDown(KeyCode.W))
                {
                    option = 2;
                    MoveSelector(keysCheatSheetGO.transform.position.y);
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    OnCloseMenu();
                }
            }
        }

        //pre: --
        //post: sets menu to initial values and makes an open animation
        public void OnOpenMenu(bool inGame)
        {
            this.inGame = inGame;
            option = 0;
            selector.transform.position = new Vector2(selector.transform.position.x, musicGO.transform.position.y);
            optionsCanvasGroup.alpha = 0;
            optionsCanvasGroup.DOFade(1, 0.25f).SetUpdate(true);
        }

        //pre: --
        //post: makes an close animation
        private void OnCloseMenu()
        {
            PlaySelectSound();
            optionsCanvasGroup.DOFade(0, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                if (inGame)
                {
                    GetComponentInParent<InGameMenuController>().CloseSettingsPage();
                }
                else
                {
                    //startmenu
                    GetComponentInParent<StartMenu>().CloseSettingsPage();
                }
            });
        }

        //pre: --
        //post: shows keyscheatsheet on screen
        private void ShowKeySheet()
        {
            PlaySelectSound();
            if (inGame)
            {
                GetComponentInChildren<ManagePowersVisibility>().ManageAdquiredPowersVisibility();
            }
            else
            {
                GetComponentInChildren<ManagePowersVisibility>().ShowAllPowers();
            }
            optionsCanvasGroup.DOFade(0, 0.25f).SetUpdate(true);
            keyCheatSheetCanvasGroup.DOFade(1, 0.25f).SetUpdate(true).OnComplete(
                () =>
                {
                    showingkeyCheatSheet = true;
                }
            );
        }

        //pre: --
        //post: hides keyscheatsheet on screen
        private void HideKeySheet()
        {
            PlaySelectSound();
            keyCheatSheetCanvasGroup.DOFade(0, 0.25f).SetUpdate(true);
            optionsCanvasGroup
                .DOFade(1, 0.25f)
                .SetUpdate(true)
                .OnComplete(() => showingkeyCheatSheet = false);
        }

        //pre: --
        //post: manages slider to increase o decrease value
        private void ManageSlider(Slider slider, bool increase)
        {
            PlaySettingSound();
            if (increase && slider.value < 1)
            {
                slider.value += slidersChangeValue;
            }
            else if (!increase && slider.value > 0)
            {
                slider.value -= slidersChangeValue;
            }
        }

        //pre: --
        //post: moves selector in yPos to show user where it is
        private void MoveSelector(float yPos)
        {
            PlayMoveSound();
            selector.transform.DOMoveY(yPos, 0.2f).SetUpdate(true);
        }

        private void PlaySettingSound()
        {
            if (audioSource == null)
            {
                Debug.LogWarning("AudioSource not found");
                return;
            }
            audioSource.clip = settingSound;
            audioSource.Play();
        }

        private void PlayMoveSound()
        {
            if (audioSource == null)
            {
                Debug.LogWarning("AudioSource not found");
                return;
            }
            audioSource.clip = navigationSound;
            audioSource.Play();
        }

        private void PlaySelectSound()
        {
            if (audioSource == null)
            {
                Debug.LogWarning("AudioSource not found");
                return;
            }
            audioSource.clip = selectSound;
            audioSource.Play();
        }
    }
}
