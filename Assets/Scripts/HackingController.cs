using System;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HackingController : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI textMeshPro;

    void Start(){
        slider = GetComponentInChildren<Slider>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }
    internal void IncreaseSlider(string v)
    {
        slider.DOValue(slider.value+1, 1);
        textMeshPro.DOFade(0, 1);
        textMeshPro.SetText(v);
        textMeshPro.DOFade(256, 1);
    }
}
