public enum TextState { IN, STAY, OUT, NONE };

public abstract class Problem
{
    public enum ProblemState { WORDING, ANSWERS };

    protected string wording;
    //protected List<int> Answers;

    public ProblemState problemState;   //State of the problem: WORDING = the wording is being shown || ANSWERS = the wording is gone and answers are being shown
    public TextState wordingState;      //State of the wording: IN = activate In animation || OUT = activate Out animation || STAY = keep showing || NONE: default
    public TextState answersState;      //State of the answers: IN = activate In animation || OUT = activate Out animation || STAY = keep showing || NONE: default

    public abstract void GenerateAnswers(int answersNum);
    public abstract bool CheckAnswer(int answerID);

    public abstract string GetAnswers(int index);
    public abstract int GetCorrectIndex();

    //Changes problem's state
    public void ChangeState()
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
