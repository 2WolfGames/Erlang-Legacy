using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class InGameCanvas : MonoBehaviour
    {
        [SerializeField] Image deathImage;

        public void ActiveDeathImage()
        {
            deathImage.DOFade(1, 2).SetDelay(1.5f);
        }
    }
}