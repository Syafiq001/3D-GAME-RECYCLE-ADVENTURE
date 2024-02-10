using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This class displays the current username along with an image and sound effect.
public class DisplayCurrentName : MonoBehaviour
{
    public TextMeshProUGUI userName; // Reference to the TextMeshProUGUI component for displaying username
    public Image yourImage; // Reference to the image to appear
    public AudioSource soundEffect; // Reference to the AudioSource component for sound effect

    // Start is called before the first frame update
    void Start()
    {
        // Invoke the ShowImageAndText method after a delay of 1 second
        Invoke("ShowImageAndText", 1f);
    }

    // Method to show image and username text
    void ShowImageAndText()
    {
        // Activate the image
        yourImage.gameObject.SetActive(true);

        // Invoke the DisplayUsername method after a delay of 0.7 seconds
        Invoke("DisplayUsername", 0.7f);
    }

    // Method to display the username
    void DisplayUsername()
    {
        // Play the sound effect
        PlaySoundEffect();

        // Get the username from PlayerPrefs and display it
        string username = PlayerPrefs.GetString("username");
        userName.text = "<color=#282828>Welcome, </color><color=#FF5733>" + username + "</color>";
    }

    // Method to play the sound effect
    void PlaySoundEffect()
    {
        soundEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // You can add update logic here if needed
    }
}
