using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class ObjectiveManager : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager script
    public GameObject scoreCanvas; // Reference to the canvas that displays the score
    public TMP_Text objectiveText; // Reference to the TextMeshPro Text displaying the objective
    public TMP_Text scoreTMPText; // Reference to the TextMeshPro Text for displaying the score
    public TMP_Text timerTMPText; // Reference to the TextMeshPro Text for displaying the timer
    public float timerDuration = 30f; // Initial timer duration (editable in the Inspector)
    private float timer; // Current timer value
    private bool timerStarted = false; // Flag to track if the timer has started
    private bool gameEnded = false; // Flag to track if the game has ended
    private DatabaseReference databaseReference; // Reference to the Firebase Database
    private bool playerEnteredTrigger = false; // Flag to track if the player entered the trigger

    // Start is called before the first frame update
    private void Start()
    {
        // Set the initial objective text using TMP
        objectiveText.text = "Collect 4 Recyclable items of each type";

        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance; // Get the default FirebaseApp instance
            if (app != null)
            {
                // Set the database reference to the root of the Firebase Realtime Database
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            } 
            else
            {
                // Log error message if Firebase initialization fails
                Debug.LogError("Firebase initialization failed"); 
            }
        });
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the game has not ended
        if (!gameEnded)
        {
            // Check if the timer has started
            if (timerStarted)
            {
                // Call CheckObjectives() to check if objectives are met
                CheckObjectives();
                // Call UpdateTimer() to update the timer
                UpdateTimer();
            }
            // If timer has not started and player entered trigger, start the timer
            else if (playerEnteredTrigger)
            {
                StartTimer();
            }
        }
    }

    // Check if objectives are met
    private void CheckObjectives()
    {
        // Check if the objectives are met
        if (AllItemsCollectedExceptUnrecyclable())
        {
            EndGame();
        }
    }

    // Check if all items except unrecyclable ones are collected
    private bool AllItemsCollectedExceptUnrecyclable()
    {
        // Check if 4 items of each type (except Unrecyclable) have been collected
        return gameManager.GetCurrentPlastic() >= 4 &&
               gameManager.GetCurrentPaper() >= 4 &&
               gameManager.GetCurrentAluminum() >= 4 &&
               gameManager.GetCurrentGlass() >= 4;
    }

    // Update the timer
    private void UpdateTimer()
    {
        // Reduce the timer by deltaTime
        timer -= Time.deltaTime;

        // Update the UI TextMeshPro element for the timer
        UpdateTimerText();

        if (timer <= 0f)
        {
            // When the timer runs out, end the game
            EndGame();
        }
    }

    // Update the UI TextMeshPro element for the timer
    private void UpdateTimerText()
    {
        // Format the timer value as minutes and seconds
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        // Display the formatted time on the TMP Text component
        timerTMPText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds);
    }

    // End the game
    private void EndGame()
    {
        gameEnded = true;

        // Disable all canvases except the scoreCanvas
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            canvas.enabled = false;
        }

        // Enable the scoreCanvas
        scoreCanvas.SetActive(true);

        // Display the final score on the TMP Text component
        int totalScore = gameManager.GetTotalScore();
        scoreTMPText.text = "Total Score: " + totalScore;
        
        // Save the total score to Firebase
        SaveTotalScoreToFirebase(totalScore);
    }

    // Save the total score to Firebase
    private void SaveTotalScoreToFirebase(int totalScore)
    {
        // Save the total score to the "user_data" node in the database
        if (databaseReference != null)
        {
            string username = PlayerPrefs.GetString("username", "defaultUsername");

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

    // Player enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Player entered the trigger
            playerEnteredTrigger = true;
            Debug.Log("Player entered the trigger");
        }
    }

    // Player exits the trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Player exited the trigger
            playerEnteredTrigger = false;
            Debug.Log("Player exited the trigger");
        }
    }

    // Start the timer when the player enters the trigger area
    private void StartTimer()
    {
        // Start the timer when this method is called
        timerStarted = true;
        timer = timerDuration;
        Debug.Log("Timer started");
    }
}
