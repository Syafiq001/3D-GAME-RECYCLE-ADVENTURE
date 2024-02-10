using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameManager.ItemType itemType; // Enum defining the type of item (Paper, Plastic, Aluminum, Glass)
    public int itemValue; // value for collecting this item
    public int itemScore; // value for collecting this item

    public AudioSource pickSound; // Reference to the AudioSource component.

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject.
        // audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with item!");

            // Play the sound.
            PlayPickSound();

            Debug.Log("Sound played!");

            // Collect the item and perform any other necessary actions.
            FindObjectOfType<GameManager>().CollectItem(itemType, itemValue, itemScore);

            Destroy(gameObject);
        }
    }

    public void PlayPickSound()
    {
        if (pickSound != null && !pickSound.isPlaying && pickSound.enabled)
        {
            pickSound.Play();
        }
    }
}
