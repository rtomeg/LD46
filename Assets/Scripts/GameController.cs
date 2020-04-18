using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        TextAsset bomberman = Resources.Load<TextAsset>("Bomberman");
        CriminalConversation bombermanConversation = JsonUtility.FromJson<CriminalConversation>(bomberman.text);
        Debug.Log(bombermanConversation);
        Debug.Log(bombermanConversation.negotiatorAnswers[1].trustConsequence);
    }
}
