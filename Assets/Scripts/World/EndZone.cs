using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    public string endSceneName = "EndScene";
    public bool showCanvasInstead = false;
    public GameObject winCanvas;

    public bool playerEnds = true;
    public bool carEnds = true;   

    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        if (!TryGetComponent<Rigidbody>(out var rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsFinisher(other)) return;

        if (showCanvasInstead && winCanvas != null)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            winCanvas.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(endSceneName);
        }
    }

    bool IsFinisher(Collider other)
    {
        // Player?
        if (playerEnds && other.CompareTag("Player")) return true;

        // Car? (Standard Assets car has CarController on the root)
        if (carEnds && other.GetComponentInParent<UnityStandardAssets.Vehicles.Car.CarController>() != null)
            return true;

        return false;
    }
}
