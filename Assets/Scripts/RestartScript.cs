using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to load/reload scenes

public class RestartScript : MonoBehaviour
{
    void Update()
    {
        // Check if the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
