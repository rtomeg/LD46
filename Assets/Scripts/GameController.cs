using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public static float dialogueSpeed = 0.01f;
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

    void Start()
    {
        TextAsset bomberman = Resources.Load<TextAsset>("Bomberman");
        CriminalConversation bombermanConversation = JsonUtility.FromJson<CriminalConversation>(bomberman.text);
        criminalStatements = bombermanConversation.criminalStatements;

        StatementPhase();

    }

    private CriminalStatement GetValidStatement()
    {
        //TODO: Should remove from list (not repeating statements);
        //TODO: if there is not valid statement, return random one
        return criminalStatements[0];
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            StopDialogAnimation();
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
        float dialogueSpeed = GameController.dialogueSpeed;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            currentDialog.maxVisibleCharacters = visibleCount;
            if (visibleCount >= totalVisibleCharacters)
            {
                DialogFinished();
                yield break;
            }
            counter += 1;
            yield return new WaitForSeconds(dialogueSpeed);
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
        currentStatement = GetValidStatement();
        currentDialog = dialogLineController.CreateDialogue(Speaker.CRIMINAL, currentStatement.statement);
        typeWriterCoroutine = AnimateText();
        StartCoroutine(typeWriterCoroutine);
    }
    private void CriminalAnswerPhase()
    {
        currentDialog = dialogLineController.CreateDialogue(Speaker.CRIMINAL, currentNegotatorAnswer.criminalResponse);
        typeWriterCoroutine = AnimateText();
        StartCoroutine(typeWriterCoroutine);

        dialogLineController.CreateDialogue(Speaker.CRIMINAL, " ");
    }

    private void PlayerChoicePhase()
    {
        playerChoicesController.UpdateOptionsText(currentStatement.negotiatorAnswers[0].dialogueOption,
        currentStatement.negotiatorAnswers[1].dialogueOption,
        currentStatement.negotiatorAnswers[2].dialogueOption
        );
    }

    public void OnPlayerChoice(int option)
    {
        currentNegotatorAnswer = currentStatement.negotiatorAnswers[option];

        currentDialog = dialogLineController.CreateDialogue(Speaker.NEGOTIATOR, currentNegotatorAnswer.dialogueOption);
        typeWriterCoroutine = AnimateText();
        playerChoicesController.HideOptions();
        StartCoroutine(typeWriterCoroutine);
    }
    public enum GamePhase
    {
        STATEMENT,
        PLAYER_CHOICE,
        CRIMINAL_ANSWER
    }
}
