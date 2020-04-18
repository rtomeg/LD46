using System;
using System.Collections.Generic;

[Serializable]
public class CriminalConversation
{
    public List<CriminalStatement> criminalStatements;

}

[Serializable]
public class CriminalStatement
{
    public string statement;
    public NegotiatorAnswer[] negotiatorAnswers;
}

[Serializable]
public class NegotiatorAnswer
{
    public string dialogueOption;
    public int wrathConsequence;
    public int trustConsequence;
    public string criminalResponse;
}

