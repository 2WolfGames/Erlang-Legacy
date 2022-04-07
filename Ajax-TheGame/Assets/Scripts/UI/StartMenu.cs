using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using Core.Shared;

public class StartMenu : MonoBehaviour
{
    [SerializeField] SkeletonGraphic skeletonGraphic;
    [SerializeField] Image worldImage;
    [SerializeField] Image cloudsImage;
    int option;


    private void Start() {
        skeletonGraphic.AnimationState.SetAnimation(1,"init",false);
        skeletonGraphic.AnimationState.AddAnimation(1,"hoverInStart",false,0);
        option = 0;
    }

    private void Update(){
        ManageOptions();
    }

    private void FixedUpdate() {
        Function.RotateGameObject(worldImage.transform,-20);
        Function.RotateGameObject(cloudsImage.transform,30);
    }

     private void ManageOptions(){
        if (option == 0)
        { //Resume
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnStartGameHoverOut();
                OnSettingsHoverIn();
                option = 1;
            } else if (Input.GetKeyDown(KeyCode.Space))
            {
                Loader.Load(Loader.Scene.lvl1);
            }
        } else if (option == 1)
        { // Settings
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnSettingsHoverOut();
                OnQuitHoverIn();
                option = 2;
            } 
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnSettingsHoverOut();
                OnStartGameHoverIn();
                option = 0;
            } 
            else if (Input.GetKeyDown(KeyCode.Space)){
                //Scene Manager
                
            }
        } 
        else 
        { //Quit
            if (Input.GetKeyDown(KeyCode.UpArrow)){
                OnQuitHoverOut();
                OnSettingsHoverIn();
                option = 1;
            } 
            else if (Input.GetKeyDown(KeyCode.Space)){
                Application.Quit();
            }
        }
    }

    #region start menu animations
    public void OnStartGameHoverIn(){
        skeletonGraphic.AnimationState.AddAnimation(1,"hoverInStart",false,0);
    }

    public void OnStartGameHoverOut() {
        skeletonGraphic.AnimationState.AddAnimation(1,"hoverOutStart",false,0);
    }

    public void OnSettingsHoverIn(){
        skeletonGraphic.AnimationState.AddAnimation(1,"hoverInOptions",false,0);
    }

    public void OnSettingsHoverOut() {
        skeletonGraphic.AnimationState.AddAnimation(1,"hoverOutOptions",false,0);
    }

    public void OnQuitHoverIn(){
        skeletonGraphic.AnimationState.AddAnimation(1,"hoverInQuit",false,0);
    }

    public void OnQuitHoverOut() {
        skeletonGraphic.AnimationState.AddAnimation(1,"hoverOutQuit",false,0);
    }
    
    #endregion
}
