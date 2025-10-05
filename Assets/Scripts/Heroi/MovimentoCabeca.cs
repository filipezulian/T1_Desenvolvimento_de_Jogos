using UnityEngine;

public class MovimentoCabeca : MonoBehaviour
{
    private float tempo = 0.0f;
    public float velocidade = 0.025f;
    public float forca = 0.075f;
    public float pontoOrigem = 0.0f;

    private Vector3 salvaPosicao;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float cortaOnda = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        salvaPosicao = transform.localPosition;

        if (horizontal == 0 && vertical == 0)
        {
            tempo = 0.0f;
        }
        else
        {
            cortaOnda = Mathf.Sin(tempo);
            tempo = tempo + velocidade;

            if (cortaOnda > Mathf.PI * 2)
            {
                tempo = tempo - (Mathf.PI * 2);
            }
        }

        if (cortaOnda != 0)
        {
            float mudaMovimentacao = cortaOnda * forca;
            float eixosTotais = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            eixosTotais = Mathf.Clamp(eixosTotais, 0.0f, 1.0f);
            mudaMovimentacao = eixosTotais * mudaMovimentacao;
            salvaPosicao.y = pontoOrigem + mudaMovimentacao;
        }
        else
        {
            salvaPosicao.y = pontoOrigem;
        }

        transform.localPosition = salvaPosicao;
    }
}
