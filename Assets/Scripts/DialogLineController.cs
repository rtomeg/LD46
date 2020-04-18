using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogLineController : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogLinePrefab;
    public Speaker speaker;
    public void CreateDialogue()//Speaker speaker, string text)
    {
        
        GameObject prefab = Instantiate(dialogLinePrefab, this.gameObject.transform);
        TextMeshProUGUI tmp = prefab.GetComponentInChildren<TextMeshProUGUI>();
        switch (speaker)
        {
            case Speaker.CRIMINAL:
                tmp.alignment = TextAlignmentOptions.MidlineLeft;
                tmp.rectTransform.SetAnchor(AnchorPresets.MiddleRight);
                tmp.rectTransform.SetPivot(PivotPresets.MiddleRight);
                tmp.rectTransform.transform.position = Vector3.zero;
                break;
            case Speaker.NEGOTIATOR:
                tmp.alignment = TextAlignmentOptions.MidlineRight;
                tmp.rectTransform.SetAnchor(AnchorPresets.MiddleLeft);
                tmp.rectTransform.SetPivot(PivotPresets.MiddleLeft);
                tmp.rectTransform.transform.position = Vector3.zero;
                break;
        }
    }

}
