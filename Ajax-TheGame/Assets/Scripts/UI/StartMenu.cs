using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

using Core.Shared;

public class StartMenu : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;

    [SerializeField] Image worldImage;
    [SerializeField] Image cloudsImage;


    private void Start() {
        skeletonGraphic.AnimationState.SetAnimation(1,"init",false);
    }

    private void FixedUpdate() {
        Function.RotateGameObject(worldImage.transform,-20);
        Function.RotateGameObject(cloudsImage.transform,30);
    }

    public void OnStartGameHoverIn(){
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverInStart",false);
    }

    public void OnStartGameHoverOut() {
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverOutStart",false);
    }

    public void OnSettingsHoverIn(){
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverInOptions",false);
    }

    public void OnSettingsHoverOut() {
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverOutOptions",false);
    }

    public void OnQuitHoverIn(){
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverInQuit",false);
    }

    public void OnQuitHoverOut() {
        skeletonGraphic.AnimationState.SetAnimation(1,"hoverOutQuit",false);
    }
}
