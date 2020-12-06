using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public enum ProblemTyp { NUMBERS, CALCULATION };

    private int encerts;           //Number of passed problems
    private int errades;           //Number of failed problems
    private int currentErrades;    //Number of current failed tries

    private float wordingTimer;    //Timer to control wording animations
    private bool wordingIn;        //Bool to control if wording is being shown
    private float answersTimer;    //Timer to control answer buttons animations
    private bool answersIn;        //Bool to control if answer buttons are being shown

    [Header("Settings")]
    [SerializeField] private int maxErrades = 2;                //Max number of tries
    [SerializeField] private int answersNum = 3;                //Number of answers to show

    private Problem problem;

    [Header("Timers")]
    [SerializeField] private float wordingTimeIn = 2f;          //Time for wording's In animation
    [SerializeField] private float wordingTimeStay = 2f;        //Time wording will be showed for
    [SerializeField] private float wordingTimeOut = 2f;         //Time for wording's Out animation

    [SerializeField] private float AnswersTimeIn = 2f;          //Time for answers' In animation
    [SerializeField] private float AnswersTimeOut = 2f;         //Time for answers' Out animation

    [Header("References")]
    public ProblemScriptableObject ProblemsData;    //ScriptableObject from where we take the problem
    public Text Wording;
    public List<GameObject> AnswersButtons;         //List of answer buttons
    public Text EncertsNum;                         //Reference to number of passed problems text
    public Text ErradesNum;                         //Reference to number of failed problems text


    // Start is called before the first frame update
    void Start()
    {
        encerts = 0;
        errades = 0;
        currentErrades = 0;

        answersIn = false;
        wordingIn = false;

        CreateProblem();
        UpdateScore();
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
                        //Wording fades in
                        if (!wordingIn)
                        {
                            StartCoroutine(WordingFade(0f, 1f, wordingTimeIn));
                            wordingIn = true;
                        }

                        //Change to next state
                        if (wordingTimer >= wordingTimeIn)
                        {
                            problem.wordingState = TextState.STAY;
                        }
                        break;

                    case TextState.STAY:
                        //Wording stays for wordingTimeStay seconds and then changes to next state
                        wordingTimer += Time.deltaTime;
                        if (wordingTimer >= wordingTimeStay)
                        {
                            problem.wordingState = TextState.OUT;
                        }
                        break;

                    case TextState.OUT:
                        //Wording fades out
                        if (wordingIn)
                        {
                            StartCoroutine(WordingFade(1f, 0f, wordingTimeOut));
                            wordingIn = false;
                        }

                        //Change problem state, so Answers can appear
                        if (wordingTimer >= wordingTimeOut)
                        {
                            problem.ChangeState();
                        }
                        break;
                }
                break;

            case Problem.ProblemState.ANSWERS:
                switch (problem.answersState)
                {
                    case TextState.IN:
                        //Answers fade in
                        if (!answersIn)
                        {
                            StartCoroutine(AllAnswerFade(0f, 1f, AnswersTimeIn));

                            answersIn = true;
                        }

                        //Change to next state
                        if (answersTimer >= AnswersTimeIn)
                            problem.answersState = TextState.STAY;
                        break;

                    case TextState.OUT:
                        //Answers fade out
                        if (answersIn)
                        {
                            StartCoroutine(AllAnswerFade(1f, 0f, AnswersTimeOut));
                            answersIn = false;
                        }

                        //New problem
                        if (answersTimer >= AnswersTimeOut)
                        {
                            ResetButtons();
                            problem.ChangeState();
                            CreateProblem();
                        }
                        break;
                }
                break;
        }
    }

    //Initializes a new problem
    void CreateProblem()
    {
        //Initialize problem
        int problemIndex = Random.Range(0, ProblemsData.Problems.Count);
        problem = new NumbersProblem(ProblemsData.Problems[problemIndex].wording, ProblemsData.Problems[problemIndex].correctAnswer);
        problem.GenerateAnswers(answersNum);

        //Set wording text
        Wording.text = ProblemsData.Problems[problemIndex].wording;

        //Set answers text
        for (int i = 0; i < answersNum; i++)
        {
            AnswersButtons[i].GetComponentInChildren<Text>().text = problem.GetAnswers(i);
        }
    }

    //Called whenever the user clicks on an answer
    public void OnAnswerClick(int index)
    {
        if (problem.answersState == TextState.STAY)
        {
            bool correct = problem.CheckAnswer(index);
            if (correct)    //if answer is correct
            {
                encerts++;
                AnswersButtons[index].GetComponent<Image>().color = Color.green;
                problem.answersState = TextState.OUT;
                currentErrades = 0;
            }
            else
            {
                //User still has tries left
                if (currentErrades < maxErrades - 1)
                {
                    AnswersButtons[index].GetComponent<Image>().color = Color.red;
                    StartCoroutine(SingleAnswerFade(index, 1f, 0f, AnswersTimeOut));

                    //User loses one try
                    currentErrades++;
                }
                else  //User is out of tries
                {
                    //Increase wrong answers
                    errades++;

                    //Change answers' state and reset current tries
                    problem.answersState = TextState.OUT;
                    AnswersButtons[index].GetComponent<Image>().color = Color.red;
                    AnswersButtons[problem.GetCorrectIndex()].GetComponent<Image>().color = Color.green;
                    currentErrades = 0;
                }
            }

            UpdateScore();
        }

    }

    //Resets buttons' color
    private void ResetButtons()
    {
        foreach (GameObject button in AnswersButtons)
        {
            button.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            button.SetActive(true);
        }
    }

    //Updates HUD score
    private void UpdateScore()
    {
        EncertsNum.text = encerts.ToString();
        ErradesNum.text = errades.ToString();
    }

    //IEnumerator to animate a single answer's button
    IEnumerator SingleAnswerFade(int index, float init, float final, float duration)
    {
        answersTimer = 0;
        while (answersTimer < duration)
        {
            answersTimer += Time.deltaTime;

            Color img = AnswersButtons[index].GetComponent<Image>().color;
            Color txt = AnswersButtons[index].GetComponentInChildren<Text>().color;

            float a = Mathf.Lerp(init, final, answersTimer / duration);

            AnswersButtons[index].GetComponent<Image>().color = new Color(img.r, img.g, img.b, a);
            AnswersButtons[index].GetComponentInChildren<Text>().color = new Color(txt.r, txt.g, txt.b, a);

            yield return null;
        }

        AnswersButtons[index].SetActive(false);
    }

    //IEnumerator to animate all answers' buttons
    IEnumerator AllAnswerFade(float init, float final, float duration)
    {
        answersTimer = 0;
        while (answersTimer < duration)
        {
            answersTimer += Time.deltaTime;

            float a = Mathf.Lerp(init, final, answersTimer / duration);

            foreach (GameObject obj in AnswersButtons)
            {
                Color img = obj.GetComponent<Image>().color;
                Color txt = obj.GetComponentInChildren<Text>().color;

                obj.GetComponent<Image>().color = new Color(img.r, img.g, img.b, a);
                obj.GetComponentInChildren<Text>().color = new Color(txt.r, txt.g, txt.b, a);
            }

            yield return null;
        }
    }

    //IEnumerator to animate wording
    IEnumerator WordingFade(float init, float final, float duration)
    {
        wordingTimer = 0;

        while (wordingTimer < duration)
        {
            wordingTimer += Time.deltaTime;

            Color img = Wording.color;
            float a = Mathf.Lerp(init, final, wordingTimer / duration);
            Wording.color = new Color(img.r, img.g, img.b, a);

            yield return null;
        }

        wordingTimer = 0;
    }
}
