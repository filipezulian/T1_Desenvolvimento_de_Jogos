using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360f, fov.distanciaVisao);

        Vector3 a = DirFromAngle(fov.transform, -fov.anguloVisao * 0.5f);
        Vector3 b = DirFromAngle(fov.transform, fov.anguloVisao * 0.5f);

        Handles.DrawLine(fov.transform.position, fov.transform.position + a * fov.distanciaVisao);
        Handles.DrawLine(fov.transform.position, fov.transform.position + b * fov.distanciaVisao);

        if (fov.podeVerPlayer)
        {
            var player = GameObject.FindWithTag("Player");
            if (player)
            {
                Handles.color = Color.green;
                Handles.DrawLine(fov.transform.position, player.transform.position);
            }
        }
    }

    Vector3 DirFromAngle(Transform t, float deg)
    {
        float rad = (deg + t.eulerAngles.y) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));
    }
}