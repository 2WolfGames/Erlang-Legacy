﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InGameCanvas : MonoBehaviour
{
    [SerializeField] Image deathImage;
    
    public void ActiveDeathImage(){
        deathImage.DOFade(1,3).SetDelay(2);
    } 
}
