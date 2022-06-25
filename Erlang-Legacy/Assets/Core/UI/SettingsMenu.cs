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
        int option = 0;
        bool showingkeyCheatSheet = false;
        bool inGame;

        // Update is called once per frame
        void Update()
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

        public void OnOpenMenu(bool inGame)
        {
            this.inGame = inGame;
            option = 0;
            selector.transform.position = new Vector2(selector.transform.position.x, musicGO.transform.position.y);
            optionsCanvasGroup.alpha = 0;
            optionsCanvasGroup.DOFade(1, 0.25f).SetUpdate(true);
        }
        private void OnCloseMenu()
        {
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

        private void ShowKeySheet()
        {
            optionsCanvasGroup.DOFade(0, 0.25f).SetUpdate(true);
            keyCheatSheetCanvasGroup.DOFade(1, 0.25f).SetUpdate(true).OnComplete(
                () =>
                {
                    showingkeyCheatSheet = true;
                }
            );
        }

        private void HideKeySheet()
        {
            keyCheatSheetCanvasGroup.DOFade(0, 0.25f).SetUpdate(true);
            optionsCanvasGroup.DOFade(1, 0.25f).SetUpdate(true).OnComplete(
                () =>
                {
                    showingkeyCheatSheet = false;
                }
            );
        }

        private void ManageSlider(Slider slider, bool increaseValue)
        {
            if (increaseValue && slider.value < 1)
            {
                slider.value += slidersChangeValue;
            }
            else if (!increaseValue && slider.value > 0)
            {
                slider.value -= slidersChangeValue;
            }
        }

        private void MoveSelector(float yPos)
        {
            selector.transform.DOMoveY(yPos, 0.2f).SetUpdate(true);
        }
    }
}