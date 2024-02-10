using UnityEngine;

public class RecycleBinScore : MonoBehaviour
{
    // Enum to specify the accepted types of bins
    public enum BinType
    {
        Paper,
        Glass,
        PlasticOrAluminum
    }

    public BinType acceptedBinType; // Specify the accepted bin type for this bin
    public int correctScore = 3; // Score to add when the correct item is thrown
    public int wrongScore = -3; // Score to deduct when the wrong item is thrown
    public int unrecyclableScore = -4; // Score to deduct when an unrecyclable item is thrown

    private GameManager gameManager; // Reference to the GameManager

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find and store the GameManager object in the scene
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the appropriate tag
        string itemTag = collision.gameObject.tag;

        // Check if the collided object is tagged as "Player"
        if (itemTag == "Player")
        {
            // Do not destroy the "Player" GameObject, and return early from the collision handling.
            return;
        }

        // Calculate the item value change based on whether the item matches the bin
        int itemValueChange = IsCorrectItemType(itemTag) ? 1 : 0;

        if (IsCorrectItemType(itemTag))
        {
            // Correct item, add points using the GameManager
            gameManager.AddScore(correctScore);
            // Increase the itemValue by the calculated value using the GameManager
            gameManager.CollectItem(GetItemTypeFromTag(itemTag), itemValueChange, IsCorrectItemType(itemTag) ? correctScore : 0);
        }
        else if (itemTag == "Unrecyclable")
        {
            // Unrecyclable item, deduct points using the GameManager
            gameManager.AddScore(unrecyclableScore);
        }
        else
        {
            // Wrong item, deduct points using the GameManager
            gameManager.AddScore(wrongScore);
        }

        // Destroy the collided item (assuming it should disappear when thrown into the bin)
        Destroy(collision.gameObject);
    }

    // Check if the collided item's tag matches the accepted type for the bin
    private bool IsCorrectItemType(string itemTag)
    {
        switch (acceptedBinType)
        {
            case BinType.Paper:
                return itemTag == "Paper";
            case BinType.Glass:
                return itemTag == "Glass";
            case BinType.PlasticOrAluminum:
                return itemTag == "Plastic" || itemTag == "Aluminum";
            default:
                return false;
        }
    }

    // Map the item's tag to the corresponding ItemType enum value
    private GameManager.ItemType GetItemTypeFromTag(string tag)
    {
        switch (tag)
        {
            case "Paper":
                return GameManager.ItemType.Paper;
            case "Plastic":
                return GameManager.ItemType.Plastic;
            case "Aluminum":
                return GameManager.ItemType.Aluminum;
            case "Glass":
                return GameManager.ItemType.Glass;
            case "Unrecyclable":
                return GameManager.ItemType.Unrecyclable;
            default:
                return GameManager.ItemType.Unrecyclable; // Default to Unrecyclable if the tag is unknown.
        }
    }
}
