using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class InGameCanvas : MonoBehaviour
    {
        [SerializeField] Image deathImage;

        //pre: --
        //post: puts transparency of death image to 1 after a period of time.
        public void ActiveDeathImage()
        {
            deathImage.DOFade(1, 3).SetDelay(2f);
        }
    }
}
