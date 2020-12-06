using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum TextState { IN, STAY, OUT, NONE };

public class Game : MonoBehaviour
{
    public enum ProblemTyp { NUMBERS, CALCULATION };

    [Header("Settings")]
    [SerializeField] private int maxErrades = 2;
    [SerializeField] private int answersNum = 3;

    [SerializeField] private float wordingTimeIn = 2f;
    [SerializeField] private float wordingTimeStay = 2f;
    [SerializeField] private float wordingTimeOut = 2f;

    [SerializeField] private float AnswersTimeIn = 2f;
    [SerializeField] private float AnswersTimeOut = 2f;

    [Header("Score")]
    [SerializeField] private int encerts;
    [SerializeField] private int errades;
    [SerializeField] private int currentErrades;


    public Problem problem;

    [Header("Timers")]
    [SerializeField] private float wordingInTimer;
    [SerializeField] private float wordingStayTimer;
    [SerializeField] private float wordingOutTimer;

    [SerializeField] private float answersInTimer;
    [SerializeField] private float answersOutTimer;

    [Header("References")]
    public ProblemScriptableObject ProblemsData;
    public Text Wording;
    public List<GameObject> AnswersButtons;
    public TextMeshProUGUI EncertsNum;
    public TextMeshProUGUI ErradesNum;


    // Start is called before the first frame update
    void Start()
    {
        encerts = 0;
        errades = 0;
        currentErrades = 0;

        CreateProblem();
        UpdateScore();

        Wording.color = new Color(255f, 255f, 255f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (problem.problemState)
        {
            case Problem.ProblemState.WORDING:
                switch (problem.wordingState)
                {
                    case TextState.IN:
                        if (wordingInTimer >= wordingTimeIn)    //Change to next state
                            problem.wordingState = TextState.STAY;
                        else
                        {
                            wordingInTimer += Time.deltaTime;

                            //Wording Fade In
                            Wording.color = new Color(Wording.color.r, Wording.color.g, Wording.color.b, FadeInOut(0f, 1f, wordingInTimer, wordingTimeIn));
                        }
                        break;

                    case TextState.STAY:
                        wordingStayTimer += Time.deltaTime;
                        if (wordingStayTimer >= wordingTimeStay)
                            problem.wordingState = TextState.OUT;
                        break;

                    case TextState.OUT:
                        if (wordingOutTimer >= wordingTimeOut)
                        {
                            problem.wordingState = TextState.NONE;
                            //ResetTimers();
                            problem.ChangeStatus();
                            //CreateProblem();    //For testing purposes
                        }
                        else
                        {
                            wordingOutTimer += Time.deltaTime;

                            //Wording Fade Out
                            Wording.color = new Color(Wording.color.r, Wording.color.g, Wording.color.b, FadeInOut(1f, 0f, wordingOutTimer, wordingTimeOut));
                        }
                        break;
                }
                break;

            case Problem.ProblemState.ANSWERS:
                switch (problem.answersState)
                {
                    case TextState.IN:
                        if (answersInTimer < AnswersTimeIn)
                        {
                            answersInTimer += Time.deltaTime;

                            foreach (GameObject obj in AnswersButtons)
                            {
                                //Fade In buttons
                                Color img = obj.GetComponent<Image>().color;
                                obj.GetComponent<Image>().color = new Color(img.r, img.g, img.b, FadeInOut(0f, 1f, answersInTimer, AnswersTimeIn));

                                Color txt = obj.GetComponentInChildren<Text>().color;
                                obj.GetComponentInChildren<Text>().color = new Color(txt.r, txt.g, txt.b, FadeInOut(0f, 1f, answersInTimer, AnswersTimeIn));
                            }
                        }
                        else
                            problem.answersState = TextState.STAY;
                        break;

                    case TextState.OUT:
                        if (answersOutTimer < AnswersTimeOut)
                        {
                            answersOutTimer += Time.deltaTime;

                            foreach (GameObject obj in AnswersButtons)
                            {
                                //Fade In buttons
                                Color img = obj.GetComponent<Image>().color;
                                obj.GetComponent<Image>().color = new Color(img.r, img.g, img.b, FadeInOut(1f, 0f, answersOutTimer, AnswersTimeOut));

                                Color txt = obj.GetComponentInChildren<Text>().color;
                                obj.GetComponentInChildren<Text>().color = new Color(txt.r, txt.g, txt.b, FadeInOut(1f, 0f, answersOutTimer, AnswersTimeOut));
                            }
                        }
                        else
                            problem.ChangeStatus();
                        break;
                }
                break;
        }
    }

    void ResetTimers()
    {
        wordingInTimer = wordingOutTimer = wordingStayTimer = 0f;
    }

    void CreateProblem()
    {
        wordingInTimer = 0;
        wordingOutTimer = 0;
        wordingStayTimer = 0;

        answersInTimer = 0;
        answersOutTimer = 0;

        //Wording.canvasRenderer.SetAlpha(0.01f);

        //Initialize problem
        int problemIndex = Random.Range(0, ProblemsData.Problems.Count);
        problem = new NumbersProblem(ProblemsData.Problems[problemIndex].wording, ProblemsData.Problems[problemIndex].correctAnswer);
        problem.GenerateAnswers(answersNum);
        //problem.InitializeProblem(ProblemsData.Problems[problemIndex], answersNum);

        //Set textmesh
        Wording.text = ProblemsData.Problems[problemIndex].wording;

    }

    public void OnAnswerClick(int index)
    {
        if (problem.answersState == TextState.STAY)
        {
            bool correct = problem.CheckAnswer(index);
            if (correct)
            {
                encerts++;
                AnswersButtons[index].GetComponent<Image>().color = Color.green;
                CreateProblem();
                problem.answersState = TextState.OUT;
                currentErrades = 0;
            }
            else
            {
                errades++;
                if (currentErrades < maxErrades - 1)
                {
                    //TODO
                    //Fade out of that option
                    currentErrades++;
                }
                else
                {
                    problem.answersState = TextState.OUT;
                    currentErrades = 0;
                }
            }

            UpdateScore();
        }

    }

    private void ResetButtons()
    {
        foreach (GameObject button in AnswersButtons)
        {
            button.GetComponent<Image>().color = Color.white;
        }
    }

    private void UpdateScore()
    {
        EncertsNum.text = encerts.ToString();
        ErradesNum.text = errades.ToString();
    }

    private float FadeInOut(float init, float final, float timer, float duration)
    {
        float a;
        a = Mathf.Lerp(init, final, timer / duration);
        return a;
    }

}
