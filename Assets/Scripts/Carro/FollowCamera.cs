using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2.2f, -5f);
    public float posLerp = 8f;
    public float rotLerp = 10f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPos = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, 1f - Mathf.Exp(-posLerp * Time.deltaTime));

        Quaternion desiredRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, 1f - Mathf.Exp(-rotLerp * Time.deltaTime));
    }
}
