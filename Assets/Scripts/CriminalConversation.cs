using System;

[Serializable]
public class CriminalConversation
{
    public string statement;
    public NegotiatorAnswers[] negotiatorAnswers;
}
[Serializable]
public class NegotiatorAnswers
{
    public string dialogueOption;
    public int wrathConsequence;
    public int trustConsequence;
    public string criminalResponse;
}

