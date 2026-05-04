using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct QuizQuestion
{
    [TextArea]
    public string questionText;
    public string[] options;
    public int correctOptionIndex;
}

[RequireComponent(typeof(BoxCollider2D))]
public class KnowledgeTerminal : MonoBehaviour
{
    [Header("Question Pool")]
    public List<QuizQuestion> questionPool = new List<QuizQuestion>();

    private bool isCompleted = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCompleted)
        {
            if (QuizManager.Instance != null && questionPool.Count > 0)
            {
                // Pick a random question from the pool
                int randomIndex = Random.Range(0, questionPool.Count);
                QuizQuestion selectedQuestion = questionPool[randomIndex];

                // Start the quiz
                QuizManager.Instance.StartQuiz(
                    selectedQuestion.questionText, 
                    selectedQuestion.options, 
                    selectedQuestion.correctOptionIndex, 
                    OnQuizSuccess
                );
            }
            else
            {
                Debug.LogError("QuizManager missing or Question Pool is empty!");
            }
        }
    }

    private void OnQuizSuccess()
    {
        isCompleted = true;
        Debug.Log("Knowledge Terminal Completed!");
        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.green; // Turn green when solved

        // Trigger Level Complete
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LevelComplete();
        }
    }
}
