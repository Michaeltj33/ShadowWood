using Photon.Pun;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public float lifeTree;
    private float getLifeTree;
    public Animator animator;

    public int valueTree;
    private int getValueTree;
    private int FinalTree;
    private int getSorteio;

    public float waitingTime;
    private float getWaitingTime;
    private PhotonView view;

    private Vector3 transformPlayer;
    private Vector3 transformTree;
    [SerializeField] private new ParticleSystem particleSystem;

    public AudioClip[] audioClip;

    private int getSpriteDrops;//icone do sprite do tronco

    private bool usedmagic;

    private void Awake()
    {
        getWaitingTime = waitingTime;
        waitingTime = 0;
        getSpriteDrops = 0;

        getLifeTree = lifeTree;
        getValueTree = valueTree;
        view = GetComponent<PhotonView>();
        transformTree = transform.position;
    }

    private void Update()
    {
        if (waitingTime > 0)
        {
            waitingTime -= Time.deltaTime;
            if (waitingTime <= 0)
            {
                // Renascer a Árvore
                lifeTree = getLifeTree;
                view.RPC("SetAnimator", RpcTarget.AllBuffered, "direction", 0);
                transform.eulerAngles = new Vector2(0, 0);
                transform.position = transformTree;
                usedmagic = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Axe") || other.CompareTag("Magic")) && lifeTree > 0)
        {
            if (other.CompareTag("Axe"))
            {
                HitOnTree(1);
                transformPlayer = other.GetComponent<AxeCollider>().GetPlayertransform();
            }
            else if (other.CompareTag("Magic"))
            {
                usedmagic = true;
                transformPlayer = other.GetComponent<Magia>().GetMagicTransform();
                HitOnTree(10);

            }
            Sound(0);
        }
    }

    private void Sound(int value)
    {
        ActorSFX.instance.PlaySfx(audioClip[value]);
    }

    public void HitOnTree(int value)
    {
        animator.SetTrigger("hut");
        particleSystem.Play();
        if (!PhotonNetwork.IsMasterClient)
        {
            return; // Apenas o MasterClient deve gerenciar a lógica de DropColectable
        }

        lifeTree -= value;

        if (lifeTree <= 0)
        {
            Sound(1);
            view.RPC("HitOnTreePun", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    public void HitOnTreePun()
    {
        float directionEuler = transformPlayer.x - transform.position.x;

        if (directionEuler < 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            transform.position = new Vector2(transform.position.x + 1.3f, transform.position.y);
        }

        waitingTime = getWaitingTime;
        view.RPC("SetAnimator", RpcTarget.AllBuffered, "direction", 1);       

        if (usedmagic)
        {
            FinalTree = 1;
        }
        else
        {
            getSorteio = ControlerColect.instance.GetQuantityObject();
            FinalTree = valueTree + getSorteio;
        }

        Vector2 vector2 = new Vector2(transform.position.x, transform.position.y - 0.8f);
        ControlerColect.instance.DropColectable(vector2, FinalTree, 0, getSpriteDrops);
    }


    [PunRPC]
    public void SetAnimator(string nome, int value)
    {
        if (animator.GetInteger(nome) != value)
        {
            animator.SetInteger(nome, value);
        }
    }
}


