using UnityEngine;
using UnityEngine.UI;

public class EndSceneHUD : MonoBehaviour
{
    public Text pontoAtualText;
    public Text maxRecordText;

    void Start()
    {
        int pontos = ScoreManager.Instance ? ScoreManager.Instance.CurrentScore
                                           : PlayerPrefs.GetInt("LAST_SCORE", 0);

        if (pontoAtualText) pontoAtualText.text = $"{pontos:000}";
        if (maxRecordText) maxRecordText.text = $"{(pontos + 10):000}";
    }
}
