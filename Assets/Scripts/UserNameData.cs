using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

// This class manages user data submission, validation, and saving to Firebase.
public class UserNameData : MonoBehaviour
{
    public TMP_InputField username; // Reference to the input field for username
    public TextMeshProUGUI warningText; // TextMeshPro component for displaying warning messages
    public TextMeshProUGUI emptyNameField; // TextMeshPro component for displaying empty name field warning
    public int Score = 0; // Public variable for score

    private DatabaseReference databaseReference; // Reference to the Firebase database

    // Start is called before the first frame update
    void Start()
    {
        // Check Firebase dependencies and initialize database reference
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            if (app != null)
            {
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError("Firebase initialization failed");
            }
        });
    }

    // Coroutine to change scene after a delay
    public IEnumerator SceneChanging()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Start");
    }

    // Method to submit user data
    public void SubmitData(string sceneName)
    {
        ClearWarnings(); // Clear warnings when the Submit button is clicked

        if (string.IsNullOrEmpty(username.text))
        {
            ShowWarningText(emptyNameField, "Enter your name");
        }
        else if (databaseReference != null)
        {
            CheckUsernameAvailability(username.text);
        }
        else
        {
            Debug.LogError("Firebase database reference is null");
        }
    }

    // Method to check if the username is available
    private void CheckUsernameAvailability(string username)
    {
        databaseReference.Child("user_data").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot != null && snapshot.Exists)
                {
                    ShowWarningText(warningText, "Username already exists. Please choose a different one.");
                }
                else
                {
                    SaveDataToFirebase(username, Score);
                }
            }
            else
            {
                Debug.LogError("Error checking username availability: " + task.Exception);
            }
        });
    }

    // Method to save user data to Firebase
    private void SaveDataToFirebase(string username, int score)
    {
        DatabaseReference userRef = databaseReference.Child("user_data").Child(username);

        // Save the username
        userRef.Child("username").SetValueAsync(username);

        // Save the score
        userRef.Child("score").SetValueAsync(score);

        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetInt("score", score);
        StartCoroutine(SceneChanging());
    }

    // Method to display warning message
    private void ShowWarningText(TextMeshProUGUI field, string message)
    {
        field.text = message;
    }

    // Method to clear warning messages
    private void ClearWarnings()
    {
        // Clear the warning texts
        warningText.text = string.Empty;
        emptyNameField.text = string.Empty;
    }
}
