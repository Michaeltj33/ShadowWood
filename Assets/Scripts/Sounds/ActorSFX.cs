using System.Linq;
using UnityEngine;
using Photon.Pun;

public class ActorSFX : MonoBehaviour
{
    public static ActorSFX instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] audioClip;


    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySfx(AudioClip clip)
    {
        /*  if (!PhotonNetwork.IsMasterClient)
         {
             return;
         } */
        audioSource.PlayOneShot(clip);
    }

    public void PlaySfx(int value)
    {
        /*  if (!PhotonNetwork.IsMasterClient)
         {
             return;
         } */
        audioSource.PlayOneShot(audioClip[value]);
    }

    public void PlaySfxOnce(AudioClip clip)
    {
        /*  if (!PhotonNetwork.IsMasterClient)
         {
             return;
         } */
        audioSource.clip = clip;
        audioSource.Play();
    }
}
