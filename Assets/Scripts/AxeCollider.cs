using Photon.Pun;
using UnityEngine;

public class AxeCollider : MonoBehaviour
{

    private Transform playertransform;
    public PhotonView view;

    private void Awake()
    {
        playertransform = GetComponentInParent<Player>().transform;
    }

    public Vector3 GetPlayertransform()
    {
        if (view.IsMine)
        {
            return playertransform.position;
        }
        return playertransform.position;
    }
}
