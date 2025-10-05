using UnityEngine;

public class AcoesObjeto : MonoBehaviour
{
    private IdentificarObjeto idObjetos;
    private bool pegou = false;

    void Start()
    {
        idObjetos = GetComponent<IdentificarObjeto>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && idObjetos.GetObjEntrar() != null)
        {
            Entrar();
        }
        
        if (Input.GetKeyDown(KeyCode.F) && idObjetos.GetObjPegar() != null)
        {
            Pegar();
        }

        if (Input.GetKeyDown(KeyCode.F) && idObjetos.GetObjArrastar() != null)
        {
            if (!pegou)
            {
                Arrastar();
            }
            else
            {
                Soltar();
            }
            pegou = !pegou;
        }
    }

    private void Entrar()
    {
        IDrivable obj = idObjetos.GetObjEntrar().GetComponent<IDrivable>();
        obj.Drive();
        idObjetos.EsconderTexto();
    }

    private void Pegar()
    {
        IPegavel obj = idObjetos.GetObjPegar().GetComponent<IPegavel>();
        if (obj != null)
        {
            obj.Pegar();
        }
        Destroy(idObjetos.GetObjPegar());
        idObjetos.EsconderTexto();
    }

    private void Arrastar()
    {
        GameObject obj = idObjetos.GetObjArrastar();
        obj.AddComponent<DragDrop>();
        obj.GetComponent<DragDrop>().Ativar();
        idObjetos.enabled = false;
    }

    private void Soltar()
    {
        GameObject obj = idObjetos.GetObjArrastar();
        Destroy(obj.GetComponent<DragDrop>());
        idObjetos.enabled = true;
    }
}
