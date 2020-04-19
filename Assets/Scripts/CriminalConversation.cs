using System;
using System.Collections.Generic;

[Serializable]
public class CriminalConversation
{
    public string introduction;
    public string firstDialog;
    public string[] hackingSteps;

    public string successEnding;
    public string failEnding;
    public List<CriminalStatement> criminalStatements;

}

[Serializable]
public class CriminalStatement
{
    public int maxTrust;
    public int minTrust;
    public string statement;
    public NegotiatorAnswer[] negotiatorAnswers;
}

[Serializable]
public class NegotiatorAnswer
{
    public string dialogueOption;
    public int trustConsequence;
    public string criminalResponse;
}

