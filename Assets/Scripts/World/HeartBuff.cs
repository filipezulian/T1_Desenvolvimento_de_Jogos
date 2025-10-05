using UnityEngine;

public class HeartBuff : MonoBehaviour, IPegavel
{
    [SerializeField] int cura = 25;
    [SerializeField] AudioClip sfx;
    [SerializeField] bool destruirAoPegar = true;

    public void Pegar()
    {
        var hero = GameObject.FindWithTag("Player")
                    ?.GetComponent<MovimentarPersonagem1>();
        if (!hero) return;

        hero.AtualizarVida(cura);

        if (sfx) AudioSource.PlayClipAtPoint(sfx, transform.position, 0.8f);
        if (destruirAoPegar) Destroy(gameObject);
    }
}
