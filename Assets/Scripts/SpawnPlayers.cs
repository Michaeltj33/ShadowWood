using Photon.Pun;
using UnityEngine;
using Cinemachine;

public class SpawnPlayers : MonoBehaviour
{
    public static SpawnPlayers instance;

    public GameObject playerPrefab;
    private Vector2 position;

    public GameObject gameObjectInicial;
    public Color[] colors;
    private PhotonView view;
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        position = gameObjectInicial.transform.position;
        instance = this;
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (view.IsMine)
        {
            view.RPC("SpawnPlayer", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void SpawnPlayer()
    {
        Vector2 randomPosition = position;

        GameObject instantiatedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        cinemachineVirtualCamera.Follow = instantiatedPlayer.transform;

    }
}
