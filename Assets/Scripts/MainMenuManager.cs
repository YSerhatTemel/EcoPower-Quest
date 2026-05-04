using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu UI")]
    public GameObject mainPanel;
    public Image playButtonBg;
    public TextMeshProUGUI playButtonText;
    public Image audioButtonBg;
    public TextMeshProUGUI audioButtonText;

    [Header("Level Select UI")]
    public GameObject levelPanel;
    public Image easyButtonBg;
    public Image mediumButtonBg;
    public Image hardButtonBg;
    public Image backButtonBg;

    private int currentState = 0; // 0 = Main Panel, 1 = Level Select Panel
    private int selectedIndex = 0;
    
    [Header("Visual Settings")]
    public Color normalColor = new Color(1f, 1f, 1f, 0.5f);
    public Color highlightColor = new Color(0.1f, 0.8f, 1.0f, 1f); // Cyan
    public float normalScale = 1.0f;
    public float highlightScale = 1.15f;
    public float animationSpeed = 15f;
    
    private bool isAudioMuted = false;
    private bool inputCooldown = false;

    void Start()
    {
        // Initialize all buttons to normal state instantly
        InitializeButtons();

        ShowMainPanel();
        UpdateAudioText();
        
        // Ensure time is running
        Time.timeScale = 1f;
    }

    void Update()
    {
        AnimateButtons();

        if (inputCooldown) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MoveSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            MoveSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelection();
        }
    }

    private void InitializeButtons()
    {
        // Reset scale and color for all buttons
        Image[] allButtons = { playButtonBg, audioButtonBg, easyButtonBg, mediumButtonBg, hardButtonBg, backButtonBg };
        foreach (var btn in allButtons)
        {
            if (btn != null)
            {
                btn.color = normalColor;
                btn.transform.localScale = Vector3.one * normalScale;
            }
        }
    }

    private void AnimateButtons()
    {
        // Main Panel Buttons
        AnimateButton(playButtonBg, currentState == 0 && selectedIndex == 0);
        AnimateButton(audioButtonBg, currentState == 0 && selectedIndex == 1);

        // Level Panel Buttons
        AnimateButton(easyButtonBg, currentState == 1 && selectedIndex == 0);
        AnimateButton(mediumButtonBg, currentState == 1 && selectedIndex == 1);
        AnimateButton(hardButtonBg, currentState == 1 && selectedIndex == 2);
        AnimateButton(backButtonBg, currentState == 1 && selectedIndex == 3);
    }

    private void AnimateButton(Image btn, bool isSelected)
    {
        if (btn == null) return;

        Color targetColor = isSelected ? highlightColor : normalColor;
        float targetScale = isSelected ? highlightScale : normalScale;

        btn.color = Color.Lerp(btn.color, targetColor, Time.unscaledDeltaTime * animationSpeed);
        
        Vector3 currentScale = btn.transform.localScale;
        Vector3 targetScaleVec = new Vector3(targetScale, targetScale, 1f);
        btn.transform.localScale = Vector3.Lerp(currentScale, targetScaleVec, Time.unscaledDeltaTime * animationSpeed);
    }

    private void MoveSelection(int direction)
    {
        selectedIndex += direction;

        if (currentState == 0)
        {
            // Main Panel has 2 buttons
            if (selectedIndex < 0) selectedIndex = 1;
            if (selectedIndex > 1) selectedIndex = 0;
        }
        else if (currentState == 1)
        {
            // Level Panel has 4 buttons (Easy, Medium, Hard, Back)
            if (selectedIndex < 0) selectedIndex = 3;
            if (selectedIndex > 3) selectedIndex = 0;
        }
        
        PlayBeep();
    }

    private void ConfirmSelection()
    {
        PlayBeep();
        StartCoroutine(Cooldown());

        if (currentState == 0)
        {
            if (selectedIndex == 0)
            {
                // Play pressed -> Go to Level Select
                ShowLevelPanel();
            }
            else if (selectedIndex == 1)
            {
                // Toggle Audio
                ToggleAudio();
            }
        }
        else if (currentState == 1)
        {
            if (selectedIndex == 0) LoadLevel(1); // Easy
            else if (selectedIndex == 1) LoadLevel(2); // Medium
            else if (selectedIndex == 2) LoadLevel(3); // Hard
            else if (selectedIndex == 3) ShowMainPanel(); // Back
        }
    }

    private void ShowMainPanel()
    {
        currentState = 0;
        selectedIndex = 0;
        mainPanel.SetActive(true);
        levelPanel.SetActive(false);
    }

    private void ShowLevelPanel()
    {
        currentState = 1;
        selectedIndex = 0;
        mainPanel.SetActive(false);
        levelPanel.SetActive(true);
    }

    private void ToggleAudio()
    {
        isAudioMuted = !isAudioMuted;
        AudioListener.volume = isAudioMuted ? 0f : 1f;
        UpdateAudioText();
    }

    private void UpdateAudioText()
    {
        if (audioButtonText != null)
        {
            audioButtonText.text = isAudioMuted ? "Audio: OFF" : "Audio: ON";
        }
    }

    private void LoadLevel(int sceneIndex)
    {
        try
        {
            SceneManager.LoadScene(sceneIndex);
        }
        catch
        {
            Debug.LogError($"Could not load scene at index {sceneIndex}. Did you add it to Build Settings?");
        }
    }

    private void PlayBeep()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayJump(); // Repurpose jump sound for UI blip
        }
    }

    private IEnumerator Cooldown()
    {
        inputCooldown = true;
        yield return new WaitForSecondsRealtime(0.2f);
        inputCooldown = false;
    }
}
