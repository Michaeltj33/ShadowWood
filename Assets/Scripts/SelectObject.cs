using UnityEngine;
using Photon.Pun;

public class SelectObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public int indexSpriteRenderer;

    public Sprite[] spriteImage;

    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        view = GetComponent<PhotonView>();


        view.RPC("SetSpriteRenderer", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SetSpriteRenderer()
    {
        spriteRenderer.sprite = spriteImage[indexSpriteRenderer];
    }

    [PunRPC]
    public void NextSpriteRenderer()
    {
        indexSpriteRenderer++;
        spriteRenderer.sprite = spriteImage[indexSpriteRenderer];
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("madeira"))
        {
            view.RPC("NextSpriteRenderer", RpcTarget.AllBuffered);
            other.GetComponent<ColectableMadeira>().SetActiveObject(false);
        }
    }
}
