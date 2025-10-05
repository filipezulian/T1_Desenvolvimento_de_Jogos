using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IdentificarObjeto : MonoBehaviour
{
    private float distanciaAlvo;
    private GameObject objArrastar, objPegar, objEntrar;
    private GameObject objAlvo;
    public Text textoTecla, textoMsg;

    // Update is called once per frame
    void Update()
    {
        // Será executado a cada 5 frames
        // Como pode chegar a 60 frames/segundo quebraria o jogo
        if (Time.frameCount % 5 == 0)
        {
            objArrastar = null;
            objPegar = null;
            objEntrar = null;

            int ignorarLayer = 7; // ignoreplayercast
            ignorarLayer = 1 << ignorarLayer;
            ignorarLayer = ~ignorarLayer; // somente o layer 7 será ignorado

            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit, 5, ignorarLayer))
            {
                distanciaAlvo = hit.distance;

                if (objAlvo != null && hit.transform.gameObject != objAlvo)
                {
                    objAlvo.GetComponent<Outline>().OutlineWidth = 0f;
                    objAlvo = null;
                }

                var enterVehicle = hit.collider.GetComponent<EnterVehicle>();
                if (hit.transform.gameObject.tag == "EntrarCarro")
                {
                    objEntrar = hit.transform.gameObject;
                    objAlvo = objEntrar;

                    textoTecla.color = new Color(248 / 255f, 248 / 255f, 13 / 255f);
                    textoMsg.color = textoTecla.color;
                    textoTecla.text = "[E]";
                    textoMsg.text = "Entrar";

                    var ol = objAlvo.GetComponent<Outline>();
                    if (ol) ol.OutlineWidth = 5f;
                }

                if (hit.transform.gameObject.tag == "Arrastar")
                {
                    objArrastar = hit.transform.gameObject;
                    objAlvo = objArrastar;

                    textoTecla.color = new Color(248 / 255f, 248 / 255f, 13 / 255f);
                    textoMsg.color = textoTecla.color;
                    textoTecla.text = "[F]";
                    textoMsg.text = "Arrastar/Soltar";
                }

                if (hit.transform.gameObject.tag == "Pegar")
                {
                    objPegar = hit.transform.gameObject;
                    objAlvo = objPegar;

                    textoTecla.color = new Color(51 / 255f, 1, 0);
                    textoMsg.color = textoTecla.color;
                    textoTecla.text = "[F]";
                    textoMsg.text = "Pegar";
                }

                if (objAlvo != null)
                {
                    objAlvo.GetComponent<Outline>().OutlineWidth = 5f;
                }
            }
            else
            {
                if (objAlvo != null)
                {
                    objAlvo.GetComponent<Outline>().OutlineWidth = 0f;
                    objAlvo = null;
                }

                EsconderTexto();
            }
        }
    }

    public float GetDistanciaAlvo()
    {
        return distanciaAlvo;
    }
    public GameObject GetObjArrastar()
    {
        return objArrastar;
    }
    public GameObject GetObjPegar()
    {
        return objPegar;
    }
    public GameObject GetObjEntrar()
    {
        return objEntrar;
    }

    public void EsconderTexto()
    {
        textoTecla.text = "";
        textoMsg.text = "";
    }
}
