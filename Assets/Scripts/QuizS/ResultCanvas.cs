using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultCanvas : MonoBehaviour
{
    public TextMeshProUGUI correctAnswersText;
    public TextMeshProUGUI scoreText;

    private int correctAnswers;
    private int totalQuestions;
    private int score;

    void Start()
    {
        // Initialize the total number of questions (assuming you know it)
        totalQuestions = 5; // Change this to the actual number of questions

        // Display the results
        DisplayResults();
    }

    public void SetResults(int correctAnswersCount)
    {
        correctAnswers = correctAnswersCount;
        CalculateScore();
        DisplayResults();
    }

    void CalculateScore()
    {
        // Each correct answer adds up 2 points
        score = correctAnswers * 2;
    }

    void DisplayResults()
    {
        // Display the number of correct answers
        correctAnswersText.text = "Correct Answers: " + correctAnswers + " / " + totalQuestions;

        // Display the score
        scoreText.text = "Score: " + score;
    }
}
