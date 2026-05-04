using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public enum Difficulty { Easy, Medium, Hard }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Difficulty currentDifficulty = Difficulty.Easy;
    public Transform lastCheckpoint;
    
    public GameObject winPanel;
    public GameObject retryPanel;
    
    // UI Elements for selection
    public Image[] optionBackgrounds; // 0 = Play Again, 1 = Main Menu
    public TextMeshProUGUI[] optionTexts;

    private bool isRespawning = false;
    private bool isEndScreenActive = false;
    private int selectedOptionIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isEndScreenActive)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                selectedOptionIndex = (selectedOptionIndex + 1) % 2;
                UpdateSelectionUI();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                selectedOptionIndex--;
                if (selectedOptionIndex < 0) selectedOptionIndex = 1;
                UpdateSelectionUI();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteSelection();
            }
        }
    }

    private void UpdateSelectionUI()
    {
        if (optionBackgrounds == null || optionBackgrounds.Length < 2) return;
        
        for (int i = 0; i < 2; i++)
        {
            if (i == selectedOptionIndex)
            {
                optionBackgrounds[i].color = Color.yellow;
                if (optionTexts[i] != null) optionTexts[i].color = Color.black;
            }
            else
            {
                optionBackgrounds[i].color = Color.white;
                if (optionTexts[i] != null) optionTexts[i].color = Color.black;
            }
        }
    }

    private void ExecuteSelection()
    {
        isEndScreenActive = false;
        Time.timeScale = 1f;

        if (selectedOptionIndex == 0)
        {
            if (isRespawning && lastCheckpoint != null)
            {
                return; // Let the coroutine respawn the player at the checkpoint
            }

            // Play Again (Restart Scene)
            lastCheckpoint = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (selectedOptionIndex == 1)
        {
            // Main Menu
            lastCheckpoint = null;
            SceneManager.LoadScene(0); // Assuming MainMenu is index 0
        }
    }

    public void RespawnPlayer(GameObject player)
    {
        if (isRespawning || isEndScreenActive) return;

        StartCoroutine(ShowRetryScreenAndRespawn(player));
    }

    private IEnumerator ShowRetryScreenAndRespawn(GameObject player)
    {
        isRespawning = true;
        
        if (AudioManager.Instance != null) AudioManager.Instance.PlayDeath();

        if (retryPanel != null) 
        {
            retryPanel.SetActive(true);
            Transform title = retryPanel.transform.Find("Title");
            if (title != null)
            {
                TextMeshProUGUI txt = title.GetComponent<TextMeshProUGUI>();
                if (txt != null)
                {
                    txt.text = "Try Again!";
                    txt.color = new Color(1f, 0.3f, 0.3f);
                }
            }
        }
        isEndScreenActive = true;
        selectedOptionIndex = 0;
        UpdateSelectionUI();
        
        // Stop player
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        // Wait until player presses Enter to execute selection.
        // We pause the routine here. Update() will handle the rest via ExecuteSelection().
        // If they pick "Play Again", ExecuteSelection reloads the scene right away.
        // If we want to actually respawn at checkpoint:
        // Wait, if they have a checkpoint, "Play Again" should respawn at checkpoint!
        
        // Wait until user makes a selection
        while(isEndScreenActive)
        {
            yield return null;
        }

        if (selectedOptionIndex == 0) // Play Again from checkpoint if available
        {
            if (retryPanel != null) retryPanel.SetActive(false);

            if (lastCheckpoint != null)
            {
                player.transform.position = lastCheckpoint.position;
                if (rb != null)
                {
                    rb.gravityScale = 2.5f; // original
                    rb.linearVelocity = Vector2.zero;
                }
                if (pc != null) pc.enabled = true;
                isRespawning = false;
            }
            // else Scene is already reloading in ExecuteSelection
        }
    }



    public void LevelComplete()
    {
        Debug.Log("Level Complete!");
        if (winPanel != null) 
        {
            winPanel.SetActive(true);
            Transform title = winPanel.transform.Find("Title");
            if (title != null)
            {
                TextMeshProUGUI txt = title.GetComponent<TextMeshProUGUI>();
                if (txt != null)
                {
                    txt.text = "Level Complete!";
                    txt.color = new Color(0.3f, 1f, 0.3f);
                }
            }
        }
        Time.timeScale = 0f; // Freeze game
        isEndScreenActive = true;
        selectedOptionIndex = 0;
        UpdateSelectionUI();
    }
}
