using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MovimentarPersonagem1 : MonoBehaviour
{
    public CharacterController controle;
    public float velocidade = 6f;
    public float alturaPulo = 4f;
    public float gravidade = -20f;
    public AudioClip somPulo;
    private AudioSource audioSrc;

    public AudioClip[] footstepSounds;
    private float stepCooldown = 0.5f;
    private float lastStepTime;
    public Transform checaChao;

    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estaNoChao;

    Vector3 velocidadeCai;

    private Transform cameraTransform;
    private bool estahAbaixado = false;
    private bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado;
    public int vida = 100;
    public int vidaMaxima = 100;
    public Slider sliderVida;
    public GameObject telaFimJogo;
    public bool estahVivo = true;
    public Text txtPontuacaoFim;
    public Text devRecord;

    private Text SafeFindText(Transform root, string path)
    {
        var t = root?.Find(path);
        return t ? t.GetComponent<Text>() : null;
    }

    private void FimDeJogo()
    {
        if (telaFimJogo != null)
        {
            if (txtPontuacaoFim == null)
                txtPontuacaoFim = SafeFindText(telaFimJogo.transform, "Image/Painel/TxtPontuacao");
            if (devRecord == null)
                devRecord = SafeFindText(telaFimJogo.transform, "Image/Painel/DevRecord");
        }

        int pontos = ScoreManager.Instance ? ScoreManager.Instance.CurrentScore : 0;

        if (txtPontuacaoFim != null)
            txtPontuacaoFim.text = $"Pontuação: {pontos}";
        if (devRecord != null)
            devRecord.text = $"Recorde: {pontos + 10f}";

        Time.timeScale = 0;

        var mainCam = Camera.main;
        var al = mainCam ? mainCam.GetComponent<AudioListener>() : null;
        if (al) al.enabled = false;

        var glock = GetComponentInChildren<Glock>();
        if (glock) glock.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (telaFimJogo) telaFimJogo.SetActive(true);
        estahVivo = false;
    }
    public void ReiniciarJogo()
    {
        ScoreManager.Instance?.ResetScore();
        Time.timeScale = 1f;
        var idx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(idx);
    }
    public void SairJogo()
    {
        Application.Quit();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        Time.timeScale = 1f;
        controle = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    private void AgacharLevantar()
    {
        if (levantarBloqueado || estaNoChao == false)
        {
            return;
        }

        estahAbaixado = !estahAbaixado;
        if (estahAbaixado)
        {
            controle.height = alturaAbaixado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraAbaixado, 0);
        }
        else
        {
            controle.height = alturaLevantado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraEmPe, 0);
        }
    }

    private void ChecarBloqueioAbaixado()
    {
        Debug.DrawRay(cameraTransform.position, Vector3.up * 1.1f, Color.red);
        RaycastHit hit;
        levantarBloqueado = Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(checaChao.position, raioEsfera);
    }

    // Update is called once per frame
    void Update()
    {
        if (!estahVivo)
        {
            return;
        }

        if (vida <= 0)
        {
            FimDeJogo();
            return;
        }

        estaNoChao = Physics.CheckSphere(checaChao.position, raioEsfera, chaoMask);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;

        controle.Move(mover * velocidade * Time.deltaTime);
        PlayFootstepSound(x, z);
        ChecarBloqueioAbaixado();

        if (!levantarBloqueado && estaNoChao && Input.GetButtonDown("Jump"))
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -0.75f * gravidade);
            audioSrc.clip = somPulo;
            audioSrc.Play();
        }

        if (!estaNoChao)
        {
            velocidadeCai.y += gravidade * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            AgacharLevantar();
        }

        controle.Move(velocidadeCai * Time.deltaTime);
    }

    private void PlayFootstepSound(float horizontal, float vertical)
    {
        if (estaNoChao && (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f))
        {
            if (Time.time - lastStepTime > stepCooldown)
            {
                if (footstepSounds.Length > 0)
                {
                    int randomIndex = Random.Range(0, footstepSounds.Length);
                    audioSrc.PlayOneShot(footstepSounds[randomIndex]);
                    lastStepTime = Time.time;
                }
            }
        }
    }

    public void AtualizarVida(int novaVida)
    {
        vida = Mathf.CeilToInt(Mathf.Clamp(vida + novaVida, 0, 100));
        sliderVida.value = vida;
    }
}
