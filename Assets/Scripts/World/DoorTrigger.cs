using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    DoorUnlockFeedback feedback;

    void Awake()
    {
        feedback = GetComponentInParent<DoorUnlockFeedback>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        feedback?.ClearAfterPass();
        gameObject.SetActive(false);
    }
}
