using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InimigoChefe : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private Animator anim;
    private GameObject player;
    private bool morreu = false;
    private bool pediuAtaque;

    public int vidaMaxima = 200;
    public int vida = 200;
    public Slider sliderVida;

    public int dano = 20;
    public float distanciaDoAtaque = 2f;
    public float velocidadeAnimacao = 1.25f;

    public AudioSource audioSrc;
    public AudioClip somPasso;
    public AudioClip somMorte;
    public float volumePasso = 0.6f;
    public float volumeAtaque = 0.9f;
    [SerializeField] int pontosAoMorrer = 30;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        agente.updateRotation = false;
        agente.speed = 5f;
        anim.speed = velocidadeAnimacao;
        if (audioSrc != null) audioSrc.spatialBlend = 1f;
    }

    void Update()
    {
        if (morreu) return;
        VaiAtrasJogador();
        OlharParaJogador();
    }

    private void VaiAtrasJogador()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist > distanciaDoAtaque)
        {
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            anim.SetBool("podeAndar", true);
            anim.SetBool("pararAtaque", false);
            pediuAtaque = false;
        }
        else
        {
            agente.isStopped = true;
            anim.SetBool("podeAndar", false);
            if (!pediuAtaque && !anim.GetCurrentAnimatorStateInfo(0).IsName("Atacando"))
            {
                anim.ResetTrigger("ataque");
                anim.SetTrigger("ataque");
                pediuAtaque = true;
            }
        }

        if (dist >= distanciaDoAtaque + 1f)
        {
            anim.SetBool("pararAtaque", true);
        }

        if (anim.GetBool("podeAndar"))
        {
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            anim.ResetTrigger("ataque");
        }
    }

    private void OlharParaJogador()
    {
        Vector3 direcao = player.transform.position - transform.position;
        direcao.y = 0f;
        if (direcao.magnitude > 0.1f)
        {
            Quaternion rotacao = Quaternion.LookRotation(direcao);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, Time.deltaTime * 5f);
        }
    }

    public void DarDano()
    {
        MovimentarPersonagem1 heroi = player.GetComponent<MovimentarPersonagem1>();
        if (heroi != null)
        {
            heroi.AtualizarVida(-dano);
        }
    }

    public void LevarDano(int danoRecebido)
    {
        if (morreu) return;

        vida = Mathf.Clamp(vida - danoRecebido, 0, vidaMaxima);
        AtualizarVida(vida);

        if (vida <= 0)
        {
            Morrer();
            return;
        }

        var st = anim.GetCurrentAnimatorStateInfo(0);
        if (!st.IsName("Ferindo"))
        {
            anim.ResetTrigger("levouTiro");
            anim.SetTrigger("levouTiro");
        }
    }

    public void AtualizarVida(int vidaAtual)
    {
        vida = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
        if (sliderVida != null)
            sliderVida.value = vida;
    }

    private void Morrer()
    {
        morreu = true;
        agente.isStopped = true;
        anim.SetBool("morreu", true);
        ScoreManager.Instance?.AddPoints(pontosAoMorrer);

        if (audioSrc != null && somMorte != null)
            audioSrc.PlayOneShot(somMorte, volumeAtaque);
        Destroy(gameObject, 4f);
        OcultarHUDChefe();
    }

    public void Passo()
    {
        if (audioSrc != null && somPasso != null)
            audioSrc.PlayOneShot(somPasso, volumePasso);
    }

    private void OcultarHUDChefe()
    {
        GameObject hud = null;

        if (sliderVida != null)
        {
            var t = sliderVida.transform;
            while (t != null)
            {
                if (t.CompareTag("HideBossSlider")) { hud = t.gameObject; break; }
                t = t.parent;
            }

            if (hud == null && sliderVida.transform.parent != null)
                hud = sliderVida.transform.parent.gameObject;
        }

        if (hud == null)
            hud = GameObject.FindGameObjectWithTag("HideBossSlider");

        if (hud != null)
            hud.SetActive(false);
    }
}
