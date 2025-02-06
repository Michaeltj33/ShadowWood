using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField creatInput;
    [SerializeField] private String sala;
    [SerializeField] private TextMeshProUGUI textError;



    public void CreateRoom()
    {
        if (creatInput.text == "")
        {
            textError.text = "Você precisa Digitar Seu nome.";
        }
        else
        {
            PhotonNetwork.NickName = creatInput.text;
            PhotonNetwork.CreateRoom(sala);
        }

    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(sala);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        JoinRoom();
    }
}
