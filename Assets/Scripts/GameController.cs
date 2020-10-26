using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour
{

    public int trust;
    [SerializeField]
    StatsController trustSlider;
    [SerializeField]
    AudioController audioController;

    [SerializeField]
    SplashScreenController splashScreenController;

    [SerializeField]
    HackingController hackingController;

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
    private CriminalConversation bombermanConversation;
    private bool gameOver;
    private bool introductionDone = false;
    [SerializeField]
    private NokiaController nokiaController;
    [SerializeField]
    private CanvasGroup m_canvasGroup;

    public string path = "english";

    private bool restartGame;

    private int numberOfAnswers = 0;

    void Start()
    {
        if (splashScreenController == null)
        {
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

    public void StartGame()
    {
        TextAsset bomberman = Resources.Load<TextAsset>(path);
        bombermanConversation = JsonUtility.FromJson<CriminalConversation>(bomberman.text);
        criminalStatements = bombermanConversation.criminalStatements;
        criminalStatements.Shuffle();
        nokiaController.MoveRight();
        m_canvasGroup.DOFade(1, 0.5f).OnComplete(() => CreateChatDialog(Speaker.NARRATOR, bombermanConversation.introduction));
    }


    private CriminalStatement GetValidStatement()
    {

        CriminalStatement answer = criminalStatements.Find(x => x.minTrust <= trust && x.maxTrust >= trust);
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
        {
            if (dialogIsPlaying) StopDialogAnimation();
            if (!gameOver && restartGame && !dialogIsPlaying) RestartGame();
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
        if (restartGame)
        {
            RestartGame();
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

                if (restartGame)
                {
                    yield break;
                }

                DialogFinished();
                yield break;
            }

            counter += 1;
            if (gameOver)
            {
                if (audioController == null)
                {
                    audioController = FindObjectOfType<AudioController>();
                }
                audioController.PlayEndCallSound();
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                if (counter % 2 == 0)
                {
                    nokiaController.Shake();
                    if (audioController == null)
                    {
                        audioController = FindObjectOfType<AudioController>();
                    }
                    audioController.PlayBeepKey();
                }
                yield return new WaitForSeconds(dialogueSpeed);
            }
        }
    }

    private void RestartGame()
    {
        m_canvasGroup.DOFade(0, 1f).SetDelay(3).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
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
        if (!introductionDone)
        {
            CreateChatDialog(Speaker.NEGOTIATOR, bombermanConversation.firstDialog);
            introductionDone = true;
            currentGamePhase = GamePhase.CRIMINAL_ANSWER;
        }
        else
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
    }
    private void StatementPhase()
    {
        ShowRandomHackingPhrase();
        numberOfAnswers++;
        if (trust >= 20 || trust <= 0 || numberOfAnswers > 7)
        {
            CreateChatDialog(Speaker.CRIMINAL, "...");
            gameOver = true;
            StartCoroutine(StartGameOver());
        }
        else
        {
            currentStatement = GetValidStatement();
            CreateChatDialog(Speaker.CRIMINAL, currentStatement.statement);
        }
    }

    private IEnumerator StartGameOver()
    {
        yield return new WaitForSeconds(3);
        gameOver = false;
        if (numberOfAnswers > 7)
        {
            gameOver = false;
            restartGame = true;
            CreateChatDialog(Speaker.NARRATOR, bombermanConversation.successEnding);
        }
        else
        {
            restartGame = true;
            gameOver = false;
            CreateChatDialog(Speaker.NARRATOR, bombermanConversation.failEnding);
        }
    }

    private void CriminalAnswerPhase()
    {
        if (!restartGame && !gameOver)
        {
            CreateChatDialog(Speaker.CRIMINAL, currentNegotatorAnswer.criminalResponse);

        }
    }

    private void PlayerChoicePhase()
    {
        if (!restartGame && !gameOver)
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
        UpdateStats();
        playerChoicesController.HideOptions();
        CreateChatDialog(Speaker.NEGOTIATOR, currentNegotatorAnswer.dialogueOption);
    }

    private void UpdateStats()
    {
        trust = trust + currentNegotatorAnswer.trustConsequence;
        trustSlider.UpdateValue(trust);
    }

    private void CreateChatDialog(Speaker speaker, string text)
    {
        currentDialog = dialogLineController.CreateDialogue(speaker, text);
        typeWriterCoroutine = AnimateText();
        StartCoroutine(typeWriterCoroutine);
    }

    private void ShowRandomHackingPhrase()
    {
        hackingController.IncreaseSlider(bombermanConversation.hackingSteps[UnityEngine.Random.Range(0, bombermanConversation.hackingSteps.Length)]);
    }

    public enum GamePhase
    {
        STATEMENT,
        PLAYER_CHOICE,
        CRIMINAL_ANSWER
    }
}
