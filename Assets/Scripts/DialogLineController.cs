using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogLineController : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogLinePrefab;
    public Speaker speaker;
    public TextMeshProUGUI CreateDialogue(Speaker speaker, string text)
    {
        GameObject prefab = Instantiate(dialogLinePrefab, this.gameObject.transform);
        TextMeshProUGUI tmp = prefab.GetComponentInChildren<TextMeshProUGUI>();
        tmp.SetText(text+"\n\n");
        switch (speaker)
        {
            case Speaker.CRIMINAL:
                prefab.GetComponent<VerticalLayoutGroup>().padding.right = 250;
                tmp.alignment = TextAlignmentOptions.MidlineLeft;
                //tmp.rectTransform.SetAnchor(AnchorPresets.MiddleRight);
                //tmp.rectTransform.SetPivot(PivotPresets.MiddleRight);
                //tmp.rectTransform.transform.position = Vector3.zero;
                break;
            case Speaker.NEGOTIATOR:
                prefab.GetComponent<VerticalLayoutGroup>().padding.right = -250;
                tmp.alignment = TextAlignmentOptions.MidlineRight;
                //tmp.rectTransform.SetAnchor(AnchorPresets.MiddleLeft);
                //tmp.rectTransform.SetPivot(PivotPresets.MiddleLeft);
                //tmp.rectTransform.transform.position = Vector3.zero;
                break;
        }
        return tmp;
    }

}
