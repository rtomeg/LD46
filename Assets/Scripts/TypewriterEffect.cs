using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    private TextMeshProUGUI m_textMeshProUGUI;

    IEnumerator Start()
    {
        m_textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();
        m_textMeshProUGUI.ForceMeshUpdate();

        int totalVisibleCharacters = m_textMeshProUGUI.textInfo.characterCount;
        int counter = 0;
        float dialogueSpeed = GameController.dialogueSpeed;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            m_textMeshProUGUI.maxVisibleCharacters = visibleCount;
            if (visibleCount >= totalVisibleCharacters)
            {
                yield break;
            }
            counter += 1;
            yield return new WaitForSeconds(dialogueSpeed);
        }
    }
}
