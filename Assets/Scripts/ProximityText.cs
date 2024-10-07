using UnityEngine;

public class ProximityText : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public float detectionRadius = 3.0f; // Distance to trigger the text visibility
    private TextMesh textMesh; // Reference to the TextMesh component

    void Start()
    {
        // Get the TextMesh component attached to this GameObject (Floating Text)
        textMesh = GetComponent<TextMesh>();

        // Set the TextMesh to be invisible initially
        if (textMesh != null)
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0); // Set alpha to 0 (invisible)
        }
    }

    void Update()
    {
        // Check the distance between the player and this object
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // If the player is within the detection radius
        if (distance < detectionRadius)
        {
            // Make the TextMesh visible
            if (textMesh != null)
            {
                textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1); // Set alpha to 1 (visible)
            }
        }
        else
        {
            // Make the TextMesh invisible
            if (textMesh != null)
            {
                textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0); // Set alpha to 0 (invisible)
            }
        }
    }
}
