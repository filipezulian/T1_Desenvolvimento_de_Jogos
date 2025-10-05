using UnityEngine;

public class DoorUnlockFeedback : MonoBehaviour
{
    public AudioSource audioSrc;
    public AudioClip unlockClip;
    public Color outlineColor = new Color(1f, 0.9f, 0.2f);
    public float outlineWidthOn = 7f;

    Outline[] outlines;
    bool shown;

    void Awake()
    {
        if (!audioSrc) audioSrc = GetComponent<AudioSource>();
        outlines = GetComponentsInChildren<Outline>(true);
        SetOutline(0f);
    }

    public void OnUnlocked()
    {
        if (shown) return;
        shown = true;

        if (audioSrc)
        {
            if (unlockClip) audioSrc.PlayOneShot(unlockClip);
            else audioSrc.Play();
        }

        foreach (var o in outlines)
            if (o) o.OutlineColor = outlineColor;

        SetOutline(outlineWidthOn);
    }

    public void ClearAfterPass() => SetOutline(0f);

    void SetOutline(float w)
    {
        if (outlines == null) return;
        foreach (var o in outlines) if (o) o.OutlineWidth = w;
    }
}
