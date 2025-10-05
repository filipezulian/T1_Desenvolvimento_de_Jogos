using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Glock : MonoBehaviour
{
    public Text textoMunicao;
    private Animator anim;
    private bool estahAtirando;
    private RaycastHit hit;

    public GameObject efeitoTiro;
    public GameObject posEfeitoTiro;
    public GameObject faisca;

    private int carregador = 3;

    private int municao = 17;

    private AudioSource somTiro;

    public AudioClip[] clips;

    public GameObject imgCursor;

    void Start()
    {
        estahAtirando = false;
        anim = GetComponent<Animator>();
        somTiro = GetComponent<AudioSource>();
        AtualizarTextoMunicao();
    }

    void Update()
    {
        if (anim.GetBool("acaoOcorrendo"))
        {
            return;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (!estahAtirando && municao > 0)
            {
                somTiro.clip = clips[0];
                municao--;
                estahAtirando = true;
                StartCoroutine(Atirando());
            }
            else
            {
                if (!estahAtirando && municao == 0 && carregador > 0)
                {
                    Recarregar();
                }
                else
                {
                    somTiro.clip = clips[2];
                    somTiro.time = 0;
                    somTiro.Play();
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Recarregar") && carregador > 0 && municao < 17)
            {
                if (carregador > 0 && municao < 17)
                {
                    Recarregar();
                }
                else
                {
                    somTiro.clip = clips[2];
                    somTiro.time = 0;
                    somTiro.Play();
                }
            }
        }

        AtualizarTextoMunicao();
        if (Input.GetButton("Fire2"))
        {
            anim.SetBool("mirar", true);
            imgCursor.SetActive(false);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45, Time.deltaTime * 10);
        }
        else
        {
            anim.SetBool("mirar", false);
            imgCursor.SetActive(true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
        }
    }

    private void AtualizarTextoMunicao()
    {
        if (textoMunicao != null)
        {
            textoMunicao.text = municao.ToString() + " / " + carregador.ToString();
        }
    }
    private void Recarregar()
    {
        somTiro.clip = clips[1];
        somTiro.time = 1.05f;
        somTiro.Play();
        anim.Play("AtirarGlock");
        municao = 17;
        carregador--;
    }

    public void AddCarregador()
    {
        carregador++;
        AtualizarTextoMunicao();
    }


    IEnumerator Atirando()
    {
        anim.Play("RecarregarGlock");
        somTiro.time = 0;
        somTiro.Play();

        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;

        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY, 0));

        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            GameObject faiscaObj = Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            if (hit.transform.tag == "Arrastar")
            {
                Vector3 direcaoBala = ray.direction;
                hit.rigidbody.AddForceAtPosition(direcaoBala * 500, hit.point);
            }
            else
            {
                if (hit.transform.tag == "LevarDano")
                {
                    ILevarDano levarDano = hit.transform.GetComponent<ILevarDano>();
                    levarDano.LevarDano(10);
                }
            }

            Destroy(faiscaObj, 0.3f);
        }

        Destroy(efeitoTiroObj, 0.3f);

        yield return new WaitForSeconds(0.3f);

        estahAtirando = false;
    }
}