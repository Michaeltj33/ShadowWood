using UnityEngine;
using Photon.Pun;

public class Magia : MonoBehaviour
{
    public float vel;
    public float limitDistance;
    private float getLimitDistance;
    private float getVel;

    private PhotonView view;
    public bool actived;
    private Animator animator;

    private Transform magicTransform;

    private void Awake()
    {
        getVel = vel;
        getLimitDistance = limitDistance;
        animator = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
        magicTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (actived)
        {
            transform.Translate(new Vector2(vel * Time.deltaTime, 0));
            limitDistance -= Time.deltaTime;

            if (limitDistance <= 0)
            {
                actived = false;
                SetActive();
            }
        }

    }

    public void SetVel(int value)
    {
        view.RPC("SetVelRPC", RpcTarget.AllBuffered, value);
    }
    [PunRPC]
    public void SetVelRPC(int value)
    {
        limitDistance = getLimitDistance;
        if (value == 0)
        {
            vel = getVel;
        }
        else
        {
            vel = -getVel;
        }
    }
    public void SetPosition(Vector2 vector2)
    {
        transform.position = vector2;
    }

    public Vector3 GetMagicTransform()
    {
        return magicTransform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (actived)
        {
            if (other.CompareTag("Protection"))
            {
                vel *= -1;
                // limitDistance = getLimitDistance;
            }
            else if (other.CompareTag("Player") || other.CompareTag("Colisor") || other.CompareTag("Magic") || other.CompareTag("Enemy") || other.CompareTag("Tree"))
            {
                DestroyMagic();
            }
        }
    }

    [PunRPC]
    public void DestroyMagic()
    {
        actived = false;
        limitDistance = 0;
        vel = 0;
        animator.SetBool("destroy", true);
    }

    //serve para deixa o objeto false
    public void SetActive()
    {
        view.RPC("SetActiveRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SetActiveRPC()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void ExecuteAction()
    {
        actived = true;
        view.RPC("ResetMagic", RpcTarget.AllBuffered);
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void ResetMagic()
    {
        animator.SetBool("destroy", false);
        actived = true;
    }

    private void OnEnable()
    {
        view.RPC("ResetMagic", RpcTarget.AllBuffered);
    }
}
