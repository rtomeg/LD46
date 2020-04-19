using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int trust;
    public int wrath;
    [SerializeField]
    StatsController trustSlider;
    [SerializeField]
    StatsController wrathSlider;

    [SerializeField]
    AudioController audioController;

    [SerializeField]
    SplashScreenController splashScreenController;

    public static float dialogueSpeed = 0.05f;
    private IEnumerator typeWriterCoroutine;
    private TextMeshProUGUI currentDialog;
    private CriminalStatement currentStatement;
    private NegotiatorAnswer currentNegotatorAnswer;
    private bool dialogIsPlaying;
    private List<CriminalStatement> criminalStatements;
    [SerializeField]
    private DialogLineController dialogLineController;
    private GamePhase currentGamePhase;
    [SerializeField]
    private PlayerChoicesController playerChoicesController;

    private bool gameOver;
    [SerializeField]
    private NokiaController nokiaController;

    void Start()
    {
        if (splashScreenController == null){
            splashScreenController = FindObjectOfType<SplashScreenController>();
        }
        if (nokiaController == null)
        {
            nokiaController = FindObjectOfType<NokiaController>();
        }
        if (audioController == null)
        {
            audioController = FindObjectOfType<AudioController>();
        }
    }

    private void StartGame(){
        TextAsset bomberman = Resources.Load<TextAsset>("Bomberman");
        CriminalConversation bombermanConversation = JsonUtility.FromJson<CriminalConversation>(bomberman.text);
        criminalStatements = bombermanConversation.criminalStatements;

        StatementPhase();
    }
    private CriminalStatement GetValidStatement()
    {

        CriminalStatement answer = criminalStatements.Find(x => x.minTrust <= trust && x.minWrath <= wrath);
        if (answer != null)
        {
            criminalStatements.Remove(answer);
        }
        else
        {
            answer = criminalStatements[0];
        }
        return answer;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        { if(dialogIsPlaying)  StopDialogAnimation();
        }
    }

    private void StopDialogAnimation()
    {
        StopCoroutine(typeWriterCoroutine);
        currentDialog.maxVisibleCharacters = currentDialog.textInfo.characterCount;
        if (dialogIsPlaying)
        {
            DialogFinished();
        }
    }

    IEnumerator AnimateText()
    {
        DialogStarted();
        currentDialog.ForceMeshUpdate();

        int totalVisibleCharacters = currentDialog.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            currentDialog.maxVisibleCharacters = visibleCount;
            if (visibleCount >= totalVisibleCharacters)
            {
                if (gameOver)
                {
                    yield break;
                }

                DialogFinished();
                yield break;
            }

            counter += 1;
            if (gameOver)
            {
                audioController.PlayEndCallSound();
                yield return new WaitForSeconds(1f);
            }
            else
            {
                if (counter % 2 == 0)
                {
                nokiaController.Shake();
                audioController.PlayBeepKey();
                                }
                yield return new WaitForSeconds(dialogueSpeed);
            }
        }
    }


    private void DialogStarted()
    {
        dialogIsPlaying = true;
    }

    private void DialogFinished()
    {
        dialogIsPlaying = false;
        StartCoroutine(NextStep());

    }

    private IEnumerator NextStep()
    {

        if ((int)currentGamePhase == Enum.GetValues(typeof(GamePhase)).Length - 1)
        {
            currentGamePhase = 0;
        }
        else
        {
            currentGamePhase++;
        }
        yield return new WaitForSeconds(1);
        switch (currentGamePhase)
        {
            case GamePhase.STATEMENT:
                StatementPhase();
                break;
            case GamePhase.PLAYER_CHOICE:
                PlayerChoicePhase();
                break;
            case GamePhase.CRIMINAL_ANSWER:
                CriminalAnswerPhase();
                break;
        }
    }
    private void StatementPhase()
    {
        if (trust >= 10 || wrath >= 10)
        {
            currentDialog = dialogLineController.CreateDialogue(Speaker.CRIMINAL, ". . .");
            typeWriterCoroutine = AnimateText();
            StartCoroutine(typeWriterCoroutine);
            gameOver = true;
            StartGameOver();
        }
        else
        {
            currentStatement = GetValidStatement();
            currentDialog = dialogLineController.CreateDialogue(Speaker.CRIMINAL, currentStatement.statement);
            typeWriterCoroutine = AnimateText();
            StartCoroutine(typeWriterCoroutine);
        }
    }

    private void StartGameOver()
    {
        Debug.Log("GAME OVER");
    }

    private void CriminalAnswerPhase()
    {
        currentDialog = dialogLineController.CreateDialogue(Speaker.CRIMINAL, currentNegotatorAnswer.criminalResponse);
        typeWriterCoroutine = AnimateText();
        StartCoroutine(typeWriterCoroutine);
    }

    private void PlayerChoicePhase()
    {
        if (!gameOver)
        {
            playerChoicesController.UpdateOptionsText(currentStatement.negotiatorAnswers[0].dialogueOption,
            currentStatement.negotiatorAnswers[1].dialogueOption,
            currentStatement.negotiatorAnswers[2].dialogueOption
            );
        }
    }

    public void OnPlayerChoice(int option)
    {
        currentNegotatorAnswer = currentStatement.negotiatorAnswers[option];

        currentDialog = dialogLineController.CreateDialogue(Speaker.NEGOTIATOR, currentNegotatorAnswer.dialogueOption);
        UpdateStats();
        typeWriterCoroutine = AnimateText();
        playerChoicesController.HideOptions();
        StartCoroutine(typeWriterCoroutine);

    }

    private void UpdateStats()
    {
        trust = trust + currentNegotatorAnswer.trustConsequence;
        wrath = wrath + currentNegotatorAnswer.wrathConsequence;

        trustSlider.UpdateValue(trust);
        wrathSlider.UpdateValue(wrath);
    }



    public enum GamePhase
    {
        STATEMENT,
        PLAYER_CHOICE,
        CRIMINAL_ANSWER
    }
}
