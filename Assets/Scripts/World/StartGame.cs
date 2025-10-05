using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void NovoJogo()
    {
        ScoreManager.Instance?.ResetScore();
        SceneManager.LoadScene(1);
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}