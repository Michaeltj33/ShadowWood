using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [Header("Player")]
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Player player;

    private bool visiblePlayer;

    public GameObject bar;
    public Image barFillAmount;

    private float currentHearth;
    public float maxHearth;
    private float getMaxHearth;

    private bool isDead;

    public int valueBone;
    public float waitingTime;

    public float WaitSecondAttack;
    private float getWaitingTime;

    public AudioClip[] audioClip;

    public Sprite[] icons;
    public SpriteRenderer spriteRenderer;

    private Vector3 vector3Enemy;

    private PhotonView view;
    private int getSorteio;

    private bool attacking;
    private int getSpriteDrops;//icone do sprite do osso

    public bool VisiblePlayer
    {
        get { return visiblePlayer; }
        set { visiblePlayer = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        getWaitingTime = waitingTime;
        waitingTime = 0;
        getSpriteDrops = 1;

        getMaxHearth = maxHearth;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        view = GetComponent<PhotonView>();

        vector3Enemy = transform.position;

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        SetSpriteRendererIcon(0);
    }

    private void Start()
    {
        SetBar(maxHearth);
    }

    public void SetPlayer(Player play)
    {
        player = play;
    }

    // Update is called once per frame
    void Update()
    {
        //a barra de Hp do Inimigo apenas será mostrado se o valor da vida for abaixo de 100%
        if (currentHearth == maxHearth || currentHearth <= 0)
        {
            bar.SetActive(false);
        }
        else
        {
            bar.SetActive(true);
        }

        if (!isDead)//apenas se o Inimigo estiver vivo
        {
            if (player != null)
            {
                visiblePlayer = player.GetComponent<Player>().GetVisible();
                if (visiblePlayer)
                {
                    navMeshAgent.SetDestination(player.transform.position);

                    if (Vector2.Distance(transform.position, player.transform.position) <= navMeshAgent.stoppingDistance && !attacking)
                    {
                        attacking = true;

                        //Chegou perto do Player
                        StartCoroutine(WaitAttack(WaitSecondAttack));
                    }
                    else
                    {
                        SetSpriteRendererIcon(1);
                        float directionEuler = player.transform.position.x - transform.position.x;

                        //muda a direção olhando para o Player
                        if (directionEuler > 0)
                        {
                            transform.eulerAngles = new Vector2(0, 0);
                            bar.transform.eulerAngles = new Vector2(0, 0);
                        }
                        else
                        {
                            transform.eulerAngles = new Vector2(0, 180);
                            bar.transform.eulerAngles = new Vector2(0, 0);

                        }

                        //seguindo o Player
                        AnimatorActiveOnceInt("direction", 1);
                    }
                }
                else
                {
                    navMeshAgent.SetDestination(vector3Enemy);
                }
            }
            else
            {
                if (transform.position != vector3Enemy)
                {
                    navMeshAgent.SetDestination(vector3Enemy);

                    if (Vector2.Distance(transform.position, vector3Enemy) <= navMeshAgent.stoppingDistance)
                    {
                        //Chegou perto do Local Inicial
                        AnimatorActiveOnceInt("direction", 0);
                    }
                    else
                    {

                        SetSpriteRendererIcon(1);

                        float directionEuler = vector3Enemy.x - transform.position.x;

                        //muda a direção olhando para o Local Inicial
                        if (directionEuler > 0)
                        {
                            transform.eulerAngles = new Vector2(0, 0);
                        }
                        else
                        {
                            transform.eulerAngles = new Vector2(0, 180);
                        }
                        //seguindo para o Local inicial
                        AnimatorActiveOnceInt("direction", 1);
                    }
                }

                SetSpriteRendererIcon(0);
            }
        }
        else
        {
            //Usado para restaurar o Inimigo
            if (waitingTime > 0)
            {
                waitingTime -= Time.deltaTime;
                if (waitingTime <= 0)
                {
                    // Faz o inimigo renacer
                    transform.position = vector3Enemy;
                    currentHearth = getMaxHearth;
                    view.RPC("SetAnimator", RpcTarget.AllBuffered, "direction", 0);
                    isDead = false;

                    float directionEuler = vector3Enemy.x - transform.position.x;

                    //muda a direção olhando para o Local Inicial
                    if (directionEuler > 0)
                    {
                        transform.eulerAngles = new Vector2(0, 0);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector2(0, 180);
                    }

                }
            }
        }
    }

    private IEnumerator WaitAttack(float value)
    {
        yield return new WaitForSeconds(value);
        animator.SetTrigger("attack");
        attacking = false;
    }

    public void RemoveBar(float value)
    {
        if (currentHearth > 0)
        {
            currentHearth -= value;
            SetFillAmout();
        }

        if (currentHearth <= 0)
        {
            isDead = true;
            AnimatorActiveOnceInt("direction", 3);
            waitingTime = getWaitingTime;

            if (!PhotonNetwork.IsMasterClient)
            {
                return; // Apenas o MasterClient deve gerenciar a lógica de DropColectable
            }

            view.RPC("HitOnEnemyPun", RpcTarget.AllBuffered);
        }
    }

    public void SetBar(float value)
    {
        currentHearth = value;
        SetFillAmout();
    }

    public void SetFillAmout()
    {
        view.RPC("TotalFillAmout", RpcTarget.AllBuffered);
    }

    public void ResetEnemy()
    {
        SetBar(maxHearth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Axe"))
        {
            animator.SetTrigger("hurt");
            RemoveBar(1);
        }
        else if (other.CompareTag("Magic"))
        {
            animator.SetTrigger("hurt");
            RemoveBar(5);
        }      
    }

    public void SetAudio(int value)
    {
        ActorSFX.instance.PlaySfx(audioClip[value]);
    }

    private void AnimatorActiveOnceInt(string anin, int value)
    {
        if (animator.GetInteger(anin) != value)
        {
            animator.SetInteger(anin, value);
        }
    }

    public void SetSpriteRendererIcon(int value)
    {
        if (spriteRenderer.sprite != icons[value])
        {
            spriteRenderer.sprite = icons[value];
        }
    }


    #region FunRPC
    [PunRPC]
    private void TotalFillAmout()
    {

        barFillAmount.fillAmount = currentHearth / maxHearth;

    }

    [PunRPC]
    public void SetAnimator(string nome, int value)
    {
        if (animator.GetInteger(nome) != value)
        {
            animator.SetInteger(nome, value);
        }
    }

    [PunRPC]
    public void HitOnEnemyPun()
    {
        if (!PhotonNetwork.IsMasterClient)
            return; // Apenas o MasterClient deve gerenciar a lógica de DropColectable

        getSorteio = ControlerColect.instance.GetQuantityObject();
        Vector2 vector2 = new Vector2(transform.position.x, transform.position.y - 0.8f);
        ControlerColect.instance.DropColectable(vector2, valueBone + getSorteio, 1, getSpriteDrops);

        waitingTime = getWaitingTime;
    }
    #endregion
}
