using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private Transform PickUpPoint; // Reference to the pick-up point transform
    private Transform player; // Reference to the player transform

    public float pickUpDistance; // Distance at which the item can be picked up
    public float forceMulti; // Multiplier for the throw force

    public bool readyToThrow; // Flag indicating if the item is ready to be thrown
    public bool itemIsPicked; // Flag indicating if the item is currently picked up
    private Rigidbody rb; // Reference to the item's rigidbody
    private Vector3 initialPosition; // Store the initial position of the object when picked up

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the rigidbody component
        player = GameObject.Find("Male")?.transform; // Find the player object and assign its transform
        PickUpPoint = GameObject.Find("PickUpPoint")?.transform; // Find the pick-up point object and assign its transform
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player and transform are not null
        if (player != null && transform != null)
        {
            // Increase throw force while holding the throw key
            if (Input.GetKey(KeyCode.E) && itemIsPicked == true && readyToThrow)
            {
                forceMulti += 500 * Time.deltaTime;
            }

            // Calculate the distance between the player and the item
            pickUpDistance = Vector3.Distance(player.position, transform.position);

            // Check if the item can be picked up and the pick-up key is pressed
            if (pickUpDistance <= 3)
            {
                if (Input.GetKeyDown(KeyCode.E) && itemIsPicked == false && PickUpPoint != null && PickUpPoint.childCount < 1)
                {
                    // Disable gravity and collider, move the item to the pick-up point, and set it as a child of the pick-up point
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<BoxCollider>().enabled = false;
                    this.transform.position = PickUpPoint.position;
                    this.transform.parent = GameObject.Find("PickUpPoint")?.transform;

                    itemIsPicked = true; // Set item as picked
                    forceMulti = 0; // Reset throw force
                }
            }

            // Check if the throw key is released while the item is picked up
            if (Input.GetKeyUp(KeyCode.E) && itemIsPicked == true)
            {
                readyToThrow = true; // Set ready to throw flag

                // Add force to the item if the throw force is sufficient
                if (forceMulti > 10)
                {
                    rb.AddForce(player.transform.forward * forceMulti); // Apply force in the player's forward direction
                    this.transform.parent = null; // Unparent the item
                    GetComponent<Rigidbody>().useGravity = true; // Enable gravity
                    GetComponent<BoxCollider>().enabled = true; // Enable collider

                    itemIsPicked = false; // Set item as not picked
                    forceMulti = 0; // Reset throw force
                }

                forceMulti = 0; // Reset throw force
            }

            // Update the position if the item is picked
            if (itemIsPicked && PickUpPoint != null)
            {
                transform.position = PickUpPoint.position; // Set the item's position to the pick-up point's position
            }
        }
    }
}
