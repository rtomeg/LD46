using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerChoicesController : MonoBehaviour
{
    private ChoiceText[] choices;
    private void Start()
    {
        choices = GetComponentsInChildren<ChoiceText>();
    }

    public void UpdateOptionsText(params string[] options)
    {

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].ShowText(options[i]);
        }
    }

    public void HideOptions()
    {
        foreach (ChoiceText choice in choices)
        {
            choice.HideText();
        }
    }
}
