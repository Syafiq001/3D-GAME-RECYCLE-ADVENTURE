using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class Objective2Manager : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager script
    public GameObject scoreCanvas; // Reference to the canvas that displays the score
    public TMP_Text objectiveText; // Reference to the TextMeshPro Text displaying the objective
    public TMP_Text scoreTMPText; // Reference to the TextMeshPro Text for displaying the score
    public TMP_Text timerTMPText; // Reference to the TextMeshPro Text for displaying the timer

    public float timerDuration = 50f; // Initial timer duration (editable in the Inspector)
    private float timer; // Current timer value
    private bool timerStarted = false; // Flag to track if the timer has started
    private bool gameEnded = false; // Flag to track if the game has ended
    private DatabaseReference databaseReference; // Reference to the Firebase database
    private bool playerEnteredTrigger = false; // Flag to track if the player entered the trigger

    private void Start()
    {
        timer = 0f; // Initialize the timer
        objectiveText.text = "Sort the material before time runs out"; // Set the initial objective text
    }

    private void Update()
    {
        if (!gameEnded)
        {
            if (timerStarted)
            {
                UpdateTimer();
            }
            else if (playerEnteredTrigger)
            {
                StartTimer();
            }
        }
    }

    private void UpdateTimer()
    {
        timer -= Time.deltaTime; // Reduce the timer by deltaTime
        UpdateTimerText(); // Update the UI TextMeshPro element for the timer

        if (timer <= 0f)
        {
            EndGame(); // When the timer runs out, end the game
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f); // Format the timer value as minutes
        int seconds = Mathf.FloorToInt(timer % 60f); // Format the timer value as seconds
        timerTMPText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds); // Display the formatted time
    }

    private void EndGame()
    {
        gameEnded = true; // Set gameEnded flag to true

        // Disable all canvases except the scoreCanvas
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            canvas.enabled = false;
        }
        scoreCanvas.SetActive(true); // Enable the scoreCanvas

        int totalScore = gameManager.GetTotalScore(); // Get the total score from GameManager
        scoreTMPText.text = "Total Score: " + totalScore; // Display the final score on the TMP Text component
        SaveTotalScoreToFirebase(totalScore); // Save the total score to Firebase
    }

    private void SaveTotalScoreToFirebase(int totalScore)
    {
        if (databaseReference != null) // Check if database reference is not null
        {
            string username = PlayerPrefs.GetString("username", "defaultUsername"); // Get username from PlayerPrefs

            // Retrieve the existing score from the database
            databaseReference.Child("user_data").Child(username).Child("score").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to read existing score from Firebase");
                    return;
                }
                DataSnapshot snapshot = task.Result;

                // Calculate the new score by adding the existing score and the new total score
                int existingScore = snapshot.Exists ? int.Parse(snapshot.Value.ToString()) : 0;
                int newScore = existingScore + totalScore;

                // Update the score in the database
                databaseReference.Child("user_data").Child(username).Child("score").SetValueAsync(newScore);
            });
        }
        else
        {
            Debug.LogError("Firebase database reference is null while saving total score");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerEnteredTrigger = true; // Set playerEnteredTrigger flag to true
            Debug.Log("Player entered the trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerEnteredTrigger = false; // Set playerEnteredTrigger flag to false
            Debug.Log("Player exited the trigger");
        }
    }

    private void StartTimer()
    {
        timerStarted = true; // Set timerStarted flag to true
        timer = timerDuration; // Set the timer duration
        Debug.Log("Timer started");
    }
}
