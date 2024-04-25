using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    private const int MaxPlayersPerScene = 2;
    private int playerCount = 0;

    public static CustomSceneManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerJoined()
    {
        playerCount++;
        if (playerCount <= MaxPlayersPerScene)
        {
            SceneManager.LoadScene("Game View", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("Countdown", LoadSceneMode.Additive);
        }
    }

    public void PlayerLeft()
    {
        playerCount--;
        // Implement logic to handle player leaving the scene if needed
    }
}