using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This class manages the quiz functionality.
public class QuizManager : MonoBehaviour
{
    public List<Question> questions; // List of questions for the quiz
    public TextMeshProUGUI questionText; // TextMeshPro component for displaying the question
    public Button[] answerButtons; // Array of buttons for displaying answer options

    public Canvas quizCanvas; // Canvas for the quiz UI
    public Canvas resultCanvas; // Canvas for displaying quiz results

    public GameManager gameManager; // Reference to the GameManager script

    private Question currentQuestion; // Current question being displayed
    private List<int> usedIndexes = new List<int>(); // List of used question indexes
    private int correctAnswers; // Counter for correct answers

    // Start is called before the first frame update
    void Start()
    {
        // Check if questions are available
        if (questions.Count > 0)
        {
            LoadRandomQuestion(); // Load a random question
        }
        else
        {
            Debug.LogError("No questions available. Please add questions to the list.");
        }
    }

    // Load a random question
    // Load a random question
    void LoadRandomQuestion()
    {
        // Check if the resultCanvas is assigned
        if (resultCanvas == null)
        {
            Debug.LogError("ResultCanvas is not assigned to QuizManager."); // Log an error if resultCanvas is not assigned
            return; // Exit the method if resultCanvas is not assigned
        }

        // Check if all questions have been answered
        if (usedIndexes.Count == questions.Count)
        {
            // All questions have been answered, close the quiz canvas and show the result canvas
            quizCanvas.gameObject.SetActive(false); // Deactivate the quiz canvas
            resultCanvas.gameObject.SetActive(true); // Activate the result canvas

            // Pass the number of correct answers to ResultCanvas
            resultCanvas.GetComponent<ResultCanvas>().SetResults(correctAnswers); // Call a method on ResultCanvas to display the results

            return; // Exit the method
        }

        int randomIndex;
        do
        {
            randomIndex = GetRandomQuestionIndex(); // Get a random index for selecting a question
        } while (usedIndexes.Contains(randomIndex)); // Repeat until a unique question index is obtained

        usedIndexes.Add(randomIndex); // Add the index to the list of used indexes
        currentQuestion = questions[randomIndex]; // Get the current question
        DisplayQuestion(); // Display the current question
    }


    // Get a random index for selecting a question
    int GetRandomQuestionIndex()
    {
        return Random.Range(0, questions.Count);
    }

    // Display the current question and answer options
    void DisplayQuestion()
    {
        // Display the question text using TextMeshPro
        questionText.text = currentQuestion.question;

        // Display the answer options randomly
        List<string> randomAnswers = new List<string>(currentQuestion.answers);
        randomAnswers.Shuffle(); // Custom method to shuffle the answers

        // Remove existing click listeners from buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].onClick.RemoveAllListeners();
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Set the answer text using TextMeshPro
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = randomAnswers[i];

            // Capture the current value of i for the lambda expression
            int index = i;

            // Add a listener to each button to handle the answer selection
            answerButtons[i].onClick.AddListener(() => AnswerSelected(randomAnswers[index]));
        }
    }

    // Handle the selection of an answer
    void AnswerSelected(string selectedAnswer)
    {
        // Check if the selected answer is correct
        bool isCorrect = selectedAnswer == currentQuestion.correctAnswer;

        // Implement your logic based on the correctness of the answer
        if (isCorrect)
        {
            Debug.Log("Correct!");
            correctAnswers++;

            // Add score to the GameManager
            gameManager.AddScore(2); // adjust the score value as needed
        }
        else
        {
            Debug.Log("Incorrect!");
        }

        // Load the next question
        LoadRandomQuestion();
    }
}
