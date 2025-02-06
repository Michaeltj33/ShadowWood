using UnityEngine;
using Photon.Pun;
using TMPro;
public class Colectable : MonoBehaviour
{
    private PhotonView view;
    private float waitingTime;
    private Animator animator;
    public Color[] color;

    public TextMeshProUGUI itemDropTotal;
    public TextMeshProUGUI itemDropTotalClone;

    public int getSpriteDrops;

    public AudioClip audioClip;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (waitingTime > 0)
        {
            waitingTime -= Time.deltaTime;

            if (waitingTime <= 0)
            {
                view.RPC("AddPoint2", RpcTarget.AllBuffered);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().AddPoint(int.Parse(itemDropTotal.text), getSpriteDrops);
            view.RPC("AddPoint2", RpcTarget.AllBuffered);
            ActorSFX.instance.PlaySfx(0); 
        }
    }

    public void SetItemDropTotal(int value)
    {
        view.RPC("SetItemDropTotalRPC", RpcTarget.AllBuffered, value);
    }


    [PunRPC]
    public void SetItemDropTotalRPC(int value)
    {
        itemDropTotal.text = value.ToString();
        itemDropTotalClone.text = value.ToString();
    }

    [PunRPC]
    public void AddPoint2()
    {
        animator.SetBool("disabled", true);
        transform.position = ControlerColect.instance.waitingPosition;
        gameObject.SetActive(false);
        //itemDropTotal colcoar quantidade de madeiras
    }


    public void SetColorText(int value)
    {
        view.RPC("SetColorTextRPC", RpcTarget.AllBuffered, value);
    }

    [PunRPC]
    public void SetColorTextRPC(int value)
    {
        itemDropTotal.color = color[value];
    }

    private void OnEnable()
    {
        waitingTime = 300f;
    }
}
