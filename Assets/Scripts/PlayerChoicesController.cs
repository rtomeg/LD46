using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerChoicesController : MonoBehaviour
{
    private TextMeshProUGUI[] texts;
    private void Start()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>(true);
    }

    public void UpdateOptionsText(params string[] options)
    {

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].gameObject.SetActive(true);
            texts[i].SetText(options[i]);
        }
    }

    public void HideOptions()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
