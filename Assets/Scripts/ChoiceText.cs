using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceText : MonoBehaviour
{
    public Color32 highlightColor;

    public Color32 mainColor;

    private TextMeshProUGUI m_textMeshProUGUI;
    private GameController gameController;

    public int choiceOption;

    private bool isVisible = false;

    void Start()
    {
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }
        m_textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        m_textMeshProUGUI.color = mainColor;
    }
    public void PointerEnter()
    {
        m_textMeshProUGUI.color = highlightColor;
    }

    public void PointerExit()
    {
        m_textMeshProUGUI.color = mainColor;
    }

    public void Clicked()
    {
        if (isVisible) gameController.OnPlayerChoice(choiceOption);
    }

    public void HideText()
    {
        isVisible = false;
        m_textMeshProUGUI.SetText("");
    }

    public void ShowText(string text)
    {
        isVisible = true;
        m_textMeshProUGUI.SetText(text);
    }
}
