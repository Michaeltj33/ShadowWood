using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // Verifica se já está conectado ao Photon
        if (PhotonNetwork.IsConnected)
        {
           return;
        }

        // Conecta ao servidor Photon
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
