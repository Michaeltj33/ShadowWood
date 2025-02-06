using Photon.Pun;
using UnityEngine;

public class PainelObjects : MonoBehaviour
{
    public static PainelObjects instance;
    public GameObject[] magias;
    private PhotonView view;

    private void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();
    }

    public void SetInMagic(Vector2 vector2, int value)
    {
        view.RPC("SetMagic", RpcTarget.AllBuffered, vector2, value);
    }
    [PunRPC]
    public void SetMagic(Vector2 position, int value)
    {
        foreach (var obg in magias)
        {
            if (!obg.activeInHierarchy)
            {
                obg.SetActive(true);
                Magia magiaScript = obg.GetComponent<Magia>();
                obg.GetComponent<Animator>().SetBool("destroy", false);
                obg.transform.position = position;
                magiaScript.actived = true;
                magiaScript.SetVel(value);
                break;
            }

        }
    }
}
