using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The prefab of the player used for spawning.
    /// </summary>
    public GameObject playerPrefab;

    /// <summary>
    /// Store the active player in the game.
    /// </summary>
    private Player activePlayer;

    /// <summary>
    /// Store the active GameManager.
    /// </summary>
    public static GameManager instance;

    private bool gamePaused;

    private void Awake()
    {
        // Check whether there is an instance
        // Check whether the instance is me
        if (instance != null && instance != this)
        {
            // If true, I'm not needed and can be destroyed.
            Destroy(gameObject);
        }
        // If not, set myself as the instance
        else
        {
            //Set the GameManager to not be destroyed when scenes are loaded.
            DontDestroyOnLoad(gameObject);

            // Subscribe the spawning function to the activeSceneChanged event.
            SceneManager.activeSceneChanged += SpawnPlayerOnLoad;

            // Set myself as the instance
            instance = this;
        }
    }

    /// <summary>
    /// Spawn the player when the scene changes
    /// </summary>
    /// <param name="currentScene"></param>
    /// <param name="nextScene"></param>
    void SpawnPlayerOnLoad(Scene currentScene, Scene nextScene)
    {
        if (nextScene.buildIndex == 0)
        {
            if (activePlayer != null)
            {
                Destroy(activePlayer);
                activePlayer = null;
            }
            return;
        }


        // Checking if there is any active player in the game.
        if (activePlayer == null)
        {
            // If there is no player, I should spawn one.
            GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

            // Store the active player.
            activePlayer = newPlayer.GetComponent<Player>();
            activePlayer.SetUp();
        }
        // If there is already a player, position the player at the right spot.
        else
        {
            // Find the spawn spot
            PlayerSpawnSpot playerSpot = FindObjectOfType<PlayerSpawnSpot>();

            // Position and rotate the player
            activePlayer.transform.position = playerSpot.transform.position;
            activePlayer.transform.rotation = playerSpot.transform.rotation;
        }
    }

    public bool GamePaused()
    {
        return gamePaused;
    }

    public void TogglePause()
    {
        if (!gamePaused)
        {
            Time.timeScale = 0f;
            gamePaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            gamePaused = false;
        }
    }
}