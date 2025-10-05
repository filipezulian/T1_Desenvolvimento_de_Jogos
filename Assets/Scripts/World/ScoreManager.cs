using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; }
    public System.Action<int> OnScoreChanged;

    Text scoreText;
    public int CurrentScore => Score;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var scoreGO = GameObject.FindWithTag("ScoreText");
        scoreText = scoreGO ? scoreGO.GetComponent<Text>() : null;
        UpdateUI();
    }

    public void ResetScore()
    {
        Score = 0;
        UpdateUI();
    }

    public void AddPoints(int pts)
    {
        Score += pts;
        OnScoreChanged?.Invoke(Score);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = $"{Score:000}";
    }
}
