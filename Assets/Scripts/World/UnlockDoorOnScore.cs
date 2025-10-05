using UnityEngine;

public class UnlockDoorOnScore : MonoBehaviour
{
    public MonoBehaviour doorComponent;
    [SerializeField] DoorUnlockFeedback feedback;

    public int requiredScore = 100;
    public bool autoOpenWhenUnlocked = true;
    bool unlocked;

    void Awake()
    {
        if (!doorComponent) doorComponent = GetComponent<MonoBehaviour>();
        if (!feedback)
            feedback = GetComponent<DoorUnlockFeedback>()
                    ?? GetComponentInChildren<DoorUnlockFeedback>()
                    ?? GetComponentInParent<DoorUnlockFeedback>();
    }

    void Update()
    {
        if (unlocked || ScoreManager.Instance == null) return;

        if (ScoreManager.Instance.Score >= requiredScore)
        {
            unlocked = true;
            UnlockDoor();
        }
    }

    void UnlockDoor()
    {
        if (!doorComponent) return;

        var t = doorComponent.GetType();

        var f = t.GetField("isLocked") ?? t.GetField("IsLocked");
        if (f != null) f.SetValue(doorComponent, false);
        else
        {
            var p = t.GetProperty("isLocked") ?? t.GetProperty("IsLocked");
            if (p != null && p.CanWrite) p.SetValue(doorComponent, false, null);
        }

        feedback?.OnUnlocked();

        if (autoOpenWhenUnlocked)
        {
            var m = t.GetMethod("Open") ?? t.GetMethod("ToggleDoor") ?? t.GetMethod("OpenDoor");
            if (m != null) m.Invoke(doorComponent, null);
        }
    }
}
