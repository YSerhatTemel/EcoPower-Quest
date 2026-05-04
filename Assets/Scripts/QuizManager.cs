using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;

    [Header("UI Elements")]
    public GameObject quizPanel;
    public TMP_Text questionText;
    public TMP_Text[] optionTexts;
    public Image[] optionBackgrounds;
    public TMP_Text timerText;

    [Header("Highlight Colors")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    private string[] currentOptions;
    private int correctOptionIndex;
    private int currentSelectedIndex = 0;
    private bool isQuizActive = false;

    private float timer = 0f;
    private bool useTimer = false;

    private System.Action onCorrectAnswer;

    void Awake()
    {
        Instance = this;
        if (quizPanel != null) quizPanel.SetActive(false);
    }

    public void StartQuiz(string question, string[] options, int correctIndex, System.Action onCorrect)
    {
        currentOptions = options;
        correctOptionIndex = correctIndex;
        onCorrectAnswer = onCorrect;
        questionText.text = question;
        currentSelectedIndex = 0;

        for (int i = 0; i < optionTexts.Length; i++)
        {
            if (i < options.Length)
            {
                optionTexts[i].text = options[i];
                optionTexts[i].gameObject.SetActive(true);
                optionBackgrounds[i].gameObject.SetActive(true);
            }
            else
            {
                optionTexts[i].gameObject.SetActive(false);
                optionBackgrounds[i].gameObject.SetActive(false);
            }
        }

        useTimer = false;
        if (timerText != null) timerText.gameObject.SetActive(false);

        if (GameManager.Instance != null && GameManager.Instance.currentDifficulty == Difficulty.Hard)
        {
            useTimer = true;
            timer = 10f; // 10 seconds limit for Hard mode
            if (timerText != null) timerText.gameObject.SetActive(true);
        }

        quizPanel.SetActive(true);
        UpdateHighlight();
        isQuizActive = true;
        
        Time.timeScale = 0f; // Pause game physics
    }

    void Update()
    {
        if (!isQuizActive) return;

        if (useTimer)
        {
            timer -= Time.unscaledDeltaTime;
            if (timerText != null) timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

            if (timer <= 0)
            {
                timer = 0;
                HandleWrongAnswer();
                return;
            }
        }

        // Navigation strictly bounded to 3 keys
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentSelectedIndex--;
            if (currentSelectedIndex < 0) currentSelectedIndex = currentOptions.Length - 1;
            UpdateHighlight();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            currentSelectedIndex++;
            if (currentSelectedIndex >= currentOptions.Length) currentSelectedIndex = 0;
            UpdateHighlight();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();
        }
    }

    private void UpdateHighlight()
    {
        for (int i = 0; i < optionBackgrounds.Length; i++)
        {
            if (i < currentOptions.Length)
            {
                optionBackgrounds[i].color = (i == currentSelectedIndex) ? selectedColor : normalColor;
            }
        }
    }

    private void CheckAnswer()
    {
        isQuizActive = false;

        if (currentSelectedIndex == correctOptionIndex)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlayWin();
            optionBackgrounds[currentSelectedIndex].color = correctColor;
            StartCoroutine(WaitAndEndSuccess());
        }
        else
        {
            HandleWrongAnswer();
        }
    }

    private void HandleWrongAnswer()
    {
        isQuizActive = false;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayError();
        optionBackgrounds[currentSelectedIndex].color = wrongColor;

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.currentDifficulty == Difficulty.Easy)
            {
                StartCoroutine(WaitAndRetry());
            }
            else
            {
                // Medium or Hard: Apply penalty
                StartCoroutine(WaitAndEndFailure());
            }
        }
        else
        {
            StartCoroutine(WaitAndRetry());
        }
    }

    private IEnumerator WaitAndEndSuccess()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        quizPanel.SetActive(false);
        Time.timeScale = 1f;
        if (onCorrectAnswer != null) onCorrectAnswer.Invoke();
    }

    private IEnumerator WaitAndEndFailure()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        quizPanel.SetActive(false);
        Time.timeScale = 1f;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && GameManager.Instance != null)
        {
            GameManager.Instance.RespawnPlayer(player);
        }
    }

    private IEnumerator WaitAndRetry()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        UpdateHighlight();
        if (useTimer) timer = 10f;
        isQuizActive = true;
    }
}
