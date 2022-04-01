using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

using Core.Shared;

public class InGameMenu : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;


    private void Start() {
        skeletonGraphic.AnimationState.SetAnimation(1,"init",false);
    }

    public void OnResumeHoverIn(){
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverInResume",false);
    }

    public void OnResumeHoverOut() {
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverOutResume",false);
    }

    public void OnSettingsHoverIn(){
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverInSettings",false);
    }

    public void OnSettingsHoverOut() {
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverOutSettings",false);
    }

    public void OnQuitHoverIn(){
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverInQuit",false);
    }

    public void OnQuitHoverOut() {
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverOutQuit",false);
    }
}
