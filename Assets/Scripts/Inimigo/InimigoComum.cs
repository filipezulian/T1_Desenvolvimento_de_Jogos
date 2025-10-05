using UnityEngine;
using UnityEngine.AI;

public class InimigoComum : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private Animator anim;
    private GameObject player;
    private FieldOfView fov;
    private PatrulharAleatorio pal;
    private bool morreu = false;
    private bool pediuAtaque;

    public int vida = 50;
    public int dano = 10;
    public float distanciaDoAtaque = 2f;

    public AudioSource audioSrc;
    public AudioClip somPasso;
    public AudioClip somMorte;
    public float volumePasso = 0.6f;
    public float volumeAtaque = 0.9f;
    [SerializeField] int pontosAoMorrer = 10;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        agente = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();
        player = GameObject.FindWithTag("Player");
        agente.updateRotation = false;
        pal = GetComponent<PatrulharAleatorio>();
    }

    void Update()
    {
        if (morreu || player == null) return;

        if (fov == null || fov.podeVerPlayer)
        {
            agente.updateRotation = false;
            VaiAtrasJogador();
            OlharParaJogador();
        }
        else
        {
            agente.updateRotation = true;
            anim.SetBool("podeAndar", false);
            pal.Andar();
        }
    }

    void VaiAtrasJogador()
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
            anim.SetBool("pararAtaque", true);

        if (anim.GetBool("podeAndar"))
        {
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            anim.ResetTrigger("ataque");
        }
    }

    void OlharParaJogador()
    {
        Vector3 direcao = player.transform.position - transform.position;
        direcao.y = 0f;
        if (direcao.magnitude > 0.1f)
        {
            Quaternion rotacao = Quaternion.LookRotation(direcao);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300f);
        }
    }

    public void DarDano()
    {
        player.GetComponent<MovimentarPersonagem1>().AtualizarVida(-dano);
    }

    public void LevarDano(int danoIn)
    {
        if (morreu) return;
        vida -= danoIn;
        if (vida <= 0) { Morrer(); return; }

        var st = anim.GetCurrentAnimatorStateInfo(0);
        if (!st.IsName("Ferindo"))
            anim.SetTrigger("levouTiro");
    }

    void Morrer()
    {
        morreu = true;
        ScoreManager.Instance?.AddPoints(pontosAoMorrer);
        if (audioSrc && somMorte) { audioSrc.clip = somMorte; audioSrc.Play(); }
        agente.isStopped = true;
        anim.SetBool("morreu", true);
        Destroy(gameObject, 3f);
    }

    public void Passo()
    {
        if (audioSrc && somPasso) audioSrc.PlayOneShot(somPasso, volumePasso);
    }
}
