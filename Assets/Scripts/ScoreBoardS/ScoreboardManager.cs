using UnityEngine; // Import Unity engine namespace
using TMPro; // Import TextMeshPro namespace for text components
using Firebase; // Import Firebase namespace for Firebase functionalities
using Firebase.Database; // Import Firebase Database namespace for database operations
using Firebase.Extensions; // Import Firebase Extensions namespace for asynchronous operations
using System.Text; // Import System.Text namespace for StringBuilder

public class ScoreboardManager : MonoBehaviour // Define ScoreboardManager class inheriting MonoBehaviour
{
    public TMP_Text rankText; // TextMeshPro component to display ranks
    public TMP_Text usernameText; // TextMeshPro component to display usernames
    public TMP_Text scoreText; // TextMeshPro component to display scores

    private DatabaseReference databaseReference; // Reference to the Firebase database

    void Start() // Start method called at the beginning
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => // Check Firebase dependencies and fix asynchronously
        {
            FirebaseApp app = FirebaseApp.DefaultInstance; // Get default Firebase instance
            if (app != null) // If Firebase instance is not null
            {
                FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false); // Disable local data persistence
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference; // Get reference to the root of the database

                if (databaseReference != null) // If database reference is not null
                {
                    RetrieveScoreboardData(); // Retrieve and display scoreboard data
                }
                else
                {
                    Debug.LogError("Firebase database reference is null"); // Log error if database reference is null
                }
            }
            else
            {
                Debug.LogError("Firebase initialization failed"); // Log error if Firebase initialization failed
            }
        });
    }

    private void RetrieveScoreboardData() // Retrieve scoreboard data from Firebase
    {
        if (databaseReference != null) // If database reference is not null
        {
            DatabaseReference scoreboardRef = databaseReference.Child("user_data"); // Get reference to "user_data" node in database

            scoreboardRef.OrderByChild("score").LimitToLast(10).GetValueAsync().ContinueWithOnMainThread(task => // Retrieve top scores in ascending order asynchronously
            {
                if (task.IsFaulted) // If task is faulted
                {
                    Debug.LogError("Failed to retrieve scoreboard data from Firebase: " + task.Exception); // Log error
                    return;
                }

                DataSnapshot snapshot = task.Result; // Get snapshot of retrieved data

                if (snapshot != null && snapshot.HasChildren) // If snapshot is not null and has children
                {
                    ProcessScoreboardData(snapshot); // Process and display the retrieved scoreboard data
                }
                else
                {
                    Debug.Log("No scoreboard data available"); // Log message if no scoreboard data is available
                }
            });
        }
        else
        {
            Debug.LogError("Firebase database reference is null while retrieving scoreboard data"); // Log error if database reference is null
        }
    }

    private void ProcessScoreboardData(DataSnapshot snapshot) // Process and display scoreboard data
    {
        var scores = new System.Collections.Generic.List<DataSnapshot>(snapshot.Children); // Create list of snapshots

        scores.Reverse(); // Reverse the order of the list to display in descending order

        StringBuilder ranks = new StringBuilder(); // StringBuilder for ranks
        StringBuilder usernames = new StringBuilder(); // StringBuilder for usernames
        StringBuilder scoresText = new StringBuilder(); // StringBuilder for scores

        int rankCounter = 1; // Initialize rank counter

        foreach (DataSnapshot userSnapshot in scores) // Iterate through snapshots
        {
            string username = userSnapshot.Key; // Get username
            int score = int.Parse(userSnapshot.Child("score").Value.ToString()); // Get score

            ranks.AppendLine($"{rankCounter}"); // Append rank to StringBuilder
            usernames.AppendLine(username); // Append username to StringBuilder
            scoresText.AppendLine(score.ToString()); // Append score to StringBuilder

            rankCounter++; // Increment rank counter
        }

        UpdateTextComponents(ranks.ToString(), usernames.ToString(), scoresText.ToString()); // Update TextMeshPro components
    }

    private void UpdateTextComponents(string ranks, string usernames, string scores) // Update TextMeshPro components with retrieved data
    {
        rankText.text = ranks; // Update ranks
        usernameText.text = usernames; // Update usernames
        scoreText.text = scores; // Update scores
    }
}
