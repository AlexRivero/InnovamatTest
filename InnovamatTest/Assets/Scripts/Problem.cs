using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct Answer
{
    public int id;
    public bool isCorrect;
    public TextMeshProUGUI text;
}

[System.Serializable]
public abstract class Problem
{
    public enum ProblemState { WORDING, ANSWERS };

    protected string wording;
    //protected List<int> Answers;

    public ProblemState problemState;
    public TextState wordingState;
    public TextState answersState;

    public abstract void GenerateAnswers(int answersNum);
    public abstract void Resolve();
    public abstract bool CheckAnswer(int answerID);

    public void ChangeStatus()
    {
        if (problemState == ProblemState.WORDING)
        {
            problemState = ProblemState.ANSWERS;
            wordingState = TextState.NONE;
            answersState = TextState.IN;
        }
        else
        {
            problemState = ProblemState.WORDING;
            answersState = TextState.NONE;
            wordingState = TextState.IN;
        }
    }
}
