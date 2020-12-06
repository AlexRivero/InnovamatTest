using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NumbersProblem : Problem
{
    [SerializeField] private List<int> Answers;
    [SerializeField] private int correctAnswer;
    [SerializeField] private int correctIndex;
    private int maxAnswerRange = 11;

    public NumbersProblem(string word, int correct)
    {
        Answers = new List<int>();
        wording = word;
        //wordingState = TextState.IN;
        //answersState = TextState.NONE;
        correctAnswer = correct;
        correctIndex = -1;
    }

    public override void GenerateAnswers(int answersNum)
    {
        Debug.Log("Correct Answer is: " + correctAnswer);

        //Chose a random index where the correct answer will be, and insert in the list
        correctIndex = Random.Range(0, answersNum);
        Debug.Log("Correct answer will be at index: " + correctIndex);

        for (int i = 0; i < answersNum; i++)
        {
            if (i == correctIndex)
            {
                Answers.Add(correctAnswer);
            }
            else
            {
                int randomAnswer;
                do
                {
                    randomAnswer = Random.Range(0, maxAnswerRange);
                } while (randomAnswer == correctAnswer);

                Answers.Add(randomAnswer);
            }

            Debug.Log("Added answer: " + Answers[i]);
        }
    }

    public override bool CheckAnswer(int answerID)
    {
        if (answerID == correctIndex)
        {
            return true;
        }

        return false;
    }

    public override void Resolve()
    {
        //TODO
    }
}
