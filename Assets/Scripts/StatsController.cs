using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatsController : MonoBehaviour
{
    public Color32 mainColor;
    public Color dangerColor;

    [SerializeField]
    Slider m_slider;
    [SerializeField]
    Image fill;
    void Start()
    {
        m_slider = GetComponent<Slider>();
    }

    public void UpdateValue(int value)
    {
        m_slider.DOValue(value, 0.5f);
        if (value > 15 || value < 5)
        {
            fill.DOColor(dangerColor, 0.5f);
        }
        else
        {
            fill.DOColor(mainColor, 0.5f);
        }
    }


}
