using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NokiaController : MonoBehaviour
{
    Tween iddle;

    void Start(){
        iddle = transform.DOLocalRotate(new Vector3(0, 0, -20), 3).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    public void Shake(){
        transform.DOShakePosition(0.05f, 0.1f);
    }
}
