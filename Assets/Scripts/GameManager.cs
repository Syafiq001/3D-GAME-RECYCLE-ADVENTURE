using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Enum defining the type of item (Paper, Plastic, Aluminum, Glass, Unrecyclable)
    public enum ItemType
    {
        Paper,
        Plastic,
        Aluminum,
        Glass,
        Unrecyclable
    }

    // TextMeshPro UI elements for displaying item quantities and total score
    public TMP_Text plasticText;
    public TMP_Text paperText;
    public TMP_Text aluminumText;
    public TMP_Text glassText;
    public TMP_Text scoreText; // Total score text added

    // Variables to track current item quantities and total score
    private int currentPlastic;
    private int currentPaper;
    private int currentAluminum;
    private int currentGlass;
    private int totalScore;

    private void Start()
    {
        // Initialize item quantities and score
        currentPlastic = 0;
        currentPaper = 0;
        currentAluminum = 0;
        currentGlass = 0;
        totalScore = 0;

        // Update the UI text elements with initial values
        UpdateItemQuantities();
        UpdateTotalScore();
    }

    // Function to collect items by type and update quantities and score
    public void CollectItem(ItemType itemType, int itemValue, int itemScore)
    {
        // Increment item quantity based on type
        switch (itemType)
        {
            case ItemType.Plastic:
                currentPlastic++;
                break;
            case ItemType.Paper:
                currentPaper++;
                break;
            case ItemType.Aluminum:
                currentAluminum++;
                break;
            case ItemType.Glass:
                currentGlass++;
                break;
            case ItemType.Unrecyclable:
                // Handle the Unrecyclable case, e.g., deduct score here
                break;
        }

        // Add item score to total score
        totalScore += itemScore;

        // Update the UI to reflect changes
        UpdateItemQuantities();
        UpdateTotalScore();
    }

    // Function to directly modify total score
    public void AddScore(int scoreChange)
    {
        // Add the score change to the total score
        totalScore += scoreChange;

        // Update the UI to reflect the change in score
        UpdateTotalScore();
    }

    // Update UI TextMeshPro elements for item quantities
    private void UpdateItemQuantities()
    {
        plasticText.text = "Plastic: " + currentPlastic;
        paperText.text = "Paper: " + currentPaper;
        aluminumText.text = "Aluminum: " + currentAluminum;
        glassText.text = "Glass: " + currentGlass;
    }

    // Update UI TextMeshPro element for total score
    private void UpdateTotalScore()
    {
        scoreText.text = "Total Score: " + totalScore;
    }

    // Get current plastic quantity
    public int GetCurrentPlastic()
    {
        return currentPlastic;
    }

    // Get current paper quantity
    public int GetCurrentPaper()
    {
        return currentPaper;
    }

    // Get current aluminum quantity
    public int GetCurrentAluminum()
    {
        return currentAluminum;
    }

    // Get current glass quantity
    public int GetCurrentGlass()
    {
        return currentGlass;
    }

    // Get total score
    public int GetTotalScore()
    {
        return totalScore;
    }
}
