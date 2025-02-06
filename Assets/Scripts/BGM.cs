using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
/*         if (!PhotonNetwork.IsMasterClient)
        {
            return;
        } */
        audioSource.Play();
    }
}
