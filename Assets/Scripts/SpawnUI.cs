using UnityEngine;
using Photon.Pun;

public class SpawnUI : MonoBehaviour
{
    public static SpawnUI instance;
    public Transform canvasTransform;
    public GameObject listPainel;
    private PhotonView view;

    private GameObject painel;//recebe o painel UI

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();

    }
    public int GetPoint(int pos)
    {
        return listPainel.GetComponent<PontosController>().Getpoint(pos);
    }

    public int GetPointQtn()
    {
        return listPainel.GetComponent<PontosController>().GetpointQtn();
    }

    public void Resetpainel(int qtn, int pos)
    {
        listPainel.GetComponent<PontosController>().Setpoint(qtn, pos);
    }

    public void Setpainel2(int qtn, int pos)
    {
        // view.RPC("AddPoints", RpcTarget.AllBuffered, value, qtn, pos);
        AddPoints(qtn, pos);
    }

    [PunRPC]
    public void AddPoints(int qnt, int pos)
    {
        listPainel.GetComponent<PontosController>().Addpoint(qnt, pos);
    }
}

