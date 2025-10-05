using UnityEngine;

public class EnterVehicle : MonoBehaviour, IDrivable
{
    public GameObject playerRoot;
    public CharacterController playerController;
    public Camera playerCamera;
    public MonoBehaviour[] playerScriptsToDisable;
    public Renderer[] playerMeshesToHide;

    public GameObject carRoot;
    public MonoBehaviour carController;
    public Camera carCamera;
    public Transform seatAnchor;
    public bool lockExit = true;
    public KeyCode exitKey = KeyCode.None;

    public AudioSource sfx;
    public AudioClip enterSfx;
    public GameObject doorOutlineOrHint;
    public GameObject[] objectsToDisableOnEnter;
    public Behaviour[] behavioursToDisableOnEnter;
    public bool IsInVehicle { get; private set; }

    public void Drive()
    {
        MovimentarPersonagem1 m = GameObject.FindWithTag("Player").GetComponent<MovimentarPersonagem1>();
        TryEnter();
    }

    void Reset()
    {
        if (!carRoot) carRoot = transform.root.gameObject;
    }

    public void TryEnter()
    {
        if (IsInVehicle) return;
        if (playerRoot && Vector3.Distance(playerRoot.transform.position, transform.position) > 3.0f) return;

        Enter();
    }

    private void Enter()
    {
        IsInVehicle = true;

        if (playerController) playerController.enabled = false;
        foreach (var s in playerScriptsToDisable) if (s) s.enabled = false;

        foreach (var go in objectsToDisableOnEnter) if (go) go.SetActive(false);
        foreach (var b in behavioursToDisableOnEnter) if (b) b.enabled = false;

        if (seatAnchor && playerRoot)
            playerRoot.transform.SetPositionAndRotation(seatAnchor.position, seatAnchor.rotation);
        foreach (var r in playerMeshesToHide) if (r) r.enabled = false;

        if (carCamera)
        {
            if (!carCamera.gameObject.activeSelf) carCamera.gameObject.SetActive(true);
            var p = carCamera.transform.parent;
            while (p != null && !p.gameObject.activeSelf) { p.gameObject.SetActive(true); p = p.parent; }

            carCamera.enabled = true;
            carCamera.targetDisplay = 0;
        }

        if (playerCamera)
        {
            if (!playerCamera.gameObject.activeSelf) playerCamera.gameObject.SetActive(true);
            playerCamera.enabled = false;
        }

        if (carController) carController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (doorOutlineOrHint) doorOutlineOrHint.SetActive(false);
        if (sfx && enterSfx) sfx.PlayOneShot(enterSfx);

        if (lockExit) exitKey = KeyCode.None;
    }

    void Update()
    {
        if (IsInVehicle && exitKey != KeyCode.None && Input.GetKeyDown(exitKey))
        {
            Exit();
        }
    }

    private void Exit()
    {
        if (!IsInVehicle) return;
        IsInVehicle = false;

        if (carController) carController.enabled = false;

        if (carCamera) carCamera.enabled = false;
        if (playerCamera) playerCamera.enabled = true;

        foreach (var r in playerMeshesToHide) if (r) r.enabled = true;
        if (playerController) playerController.enabled = true;
        foreach (var s in playerScriptsToDisable) if (s) s.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
