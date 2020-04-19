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
    Slider trustSlider;
    [SerializeField]
    Slider wrathSlider;

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
        UpdateStats();
        typeWriterCoroutine = AnimateText();
        playerChoicesController.HideOptions();
        StartCoroutine(typeWriterCoroutine);

    }

    private void UpdateStats()
    {
        trust = trust + currentNegotatorAnswer.trustConsequence;
        wrath = wrath + currentNegotatorAnswer.wrathConsequence;

        trustSlider.value = trust;
        wrathSlider.value = wrath;
    }

    public enum GamePhase
    {
        STATEMENT,
        PLAYER_CHOICE,
        CRIMINAL_ANSWER
    }
}
