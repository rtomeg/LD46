using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SplashScreenController : MonoBehaviour
{
    public Color32 highlightColor;
    float textSpeed;
    Canvas m_canvas;
    TextMeshProUGUI titleText;
    [SerializeField]
    AudioController audioController;
    [SerializeField]
    private GameObject playButtom;

    [SerializeField]
    private GameController gameController;


    IEnumerator Start()
    {
        if (audioController == null)
        {
            audioController = FindObjectOfType<AudioController>();
        }

        if(gameController == null){
            gameController = FindObjectOfType<GameController>();
        }
        m_canvas = GetComponent<Canvas>();

        titleText = GetComponentInChildren<TextMeshProUGUI>();
        titleText.ForceMeshUpdate();
        int totalVisibleCHaracters = titleText.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCHaracters + 1);
            titleText.maxVisibleCharacters = visibleCount;
            if (visibleCount >= totalVisibleCHaracters)
            {
                audioController.PlayBeepKey();
                ScrollAndPlay();
                yield break;
            }
            audioController.PlayBeepKey();
            counter += 1;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void ScrollAndPlay()
    {
        transform.GetChild(0).transform.DOLocalMoveY(100, 1f).OnComplete(EnablePlayButton);
    }

    private void EnablePlayButton(){
        playButtom.SetActive(true);
        playButtom.transform.DOScale(2, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnButtonEnter(TextMeshProUGUI tmp){
        tmp.color = highlightColor;
    }

    public void OnButtonExit(TextMeshProUGUI tmp){
        tmp.color = new Color32(255, 255, 255, 255);
    }

    public void OpenTwitter(){
        Application.OpenURL("https://twitter.com/Beleitax");
    }

    public void OnPlayButtonClick(){
        GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(()=> {m_canvas.enabled = false; gameController.StartGame();});
    }
}
