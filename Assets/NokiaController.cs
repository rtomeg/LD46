using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NokiaController : MonoBehaviour
{
    Tween iddle;

    void Start(){
        iddle = transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, -10), 3).SetLoops(-1, LoopType.Yoyo);
    }
    public void Shake(){
        //myShake.Restart();
        transform.DOShakePosition(0.1f, 1);
    }
}
