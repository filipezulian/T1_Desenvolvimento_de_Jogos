using UnityEngine;
public class MovimentarPersonagem1 : MonoBehaviour
{
    public CharacterController controle;
    public float velocidade = 6f;
    public float alturaPulo = 4f;
    public float gravidade = -20f;

    public Transform checaChao;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estaNoChao;

    Vector3 velocidadeCai;

    private Transform cameraTransform;
    private bool estahAbaixado = false;
    private bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        estaNoChao = Physics.CheckSphere(checaChao.position, raioEsfera, chaoMask);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;

        controle.Move(mover * velocidade * Time.deltaTime);
        ChecarBloqueioAbaixado();
        if (!levantarBloqueado && estaNoChao && Input.GetButtonDown("Jump"))
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -0.75f * gravidade);
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
}
