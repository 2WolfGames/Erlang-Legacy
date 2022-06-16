using System.Collections;
using Core.Shared.Enum;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class KeyIndicatorDisposer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI function;
    [SerializeField] TextMeshProUGUI key;
    CanvasGroup canvasGroup => GetComponentInChildren<CanvasGroup>();
    //ButtonAnimation buttonAnimation => GetComponentInChildren<CanvasGroup>();

    bool showing = false;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0;
    }

    void Update() {
        if (showing && Input.GetKeyDown(key.text.ToLower())){
            HideTutorial();
        }
    }

    public void ShowTutorial(GameKey gameKey, string functionallity){
        function.text = functionallity;
        key.text = gameKey.ToString().ToUpper();
        showing = true;
        canvasGroup.DOFade(1,0.25f);
    }

    public void HideTutorial(){
        showing = false;
        canvasGroup.DOFade(0,0.25f);
    }
}
