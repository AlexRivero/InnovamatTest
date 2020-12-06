using System.Collections.Generic;
using UnityEngine;

public class NumbersProblem : Problem
{
    private List<int> Answers;              //List of answers
    private int correctAnswer;              //Correct answer to the problem
    private int correctIndex;               //Index of Answers' List where the correct answer is
    private int maxAnswerRange = 11;        //MaxNum for random answers

    //Constructor
    public NumbersProblem(string word, int correct)
    {
        Answers = new List<int>();
        wording = word;
        correctAnswer = correct;
        correctIndex = -1;
    }

    //Creates random answers and put both (random and correct) in the Answers list
    public override void GenerateAnswers(int answersNum)
    {
        //Chose a random index where the correct answer will be, and insert in the list
        correctIndex = Random.Range(0, answersNum);

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
                } while (randomAnswer == correctAnswer || Answers.Contains(randomAnswer));

                Answers.Add(randomAnswer);
            }
        }
    }

    //Checks if the selected answer is correct
    public override bool CheckAnswer(int answerID)
    {
        if (answerID == correctIndex)
        {
            return true;
        }

        return false;
    }

    //Returns an answer from Answers (at given index)
    public override string GetAnswers(int index)
    {
        return Answers[index].ToString();
    }

    //Returns the correct answer's index
    public override int GetCorrectIndex()
    {
        return correctIndex;
    }
}
