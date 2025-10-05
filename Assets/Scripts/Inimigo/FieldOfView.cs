using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float distanciaVisao = 10f;
    [Range(0, 360)] public float anguloVisao = 90f;
    public LayerMask mascaraAlvo;
    public LayerMask mascaraObstaculos;

    [HideInInspector] public bool podeVerPlayer;

    Transform player;

    void Start()
    {
        var go = GameObject.FindWithTag("Player");
        if (go) player = go.transform;
        StartCoroutine(RotinaProcurar());
    }

    IEnumerator RotinaProcurar()
    {
        var wait = new WaitForSeconds(0.2f);
        while (true)
        {
            ProcurarPlayerVisivel();
            yield return wait;
        }
    }

    public void ProcurarPlayerVisivel()
    {
        podeVerPlayer = false;
        if (!player) return;

        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0f;

        if (Vector3.Angle(transform.forward, dir) > anguloVisao * 0.5f) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (!Physics.Raycast(transform.position, dir, dist, mascaraObstaculos))
            podeVerPlayer = dist <= distanciaVisao;
    }
}