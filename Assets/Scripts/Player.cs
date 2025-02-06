using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class Player : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRendererIcon;
    public new ParticleSystem particleSystem;

    public GameObject barName;
    public Sprite[] icons;
    public GameObject bar;
    public Image barFillAmount;
    private Animator animator;
    private Vector2 direction;
    private int direct;
    private float directDistance;
    public float resurrectionTime;
    public float getResurrectionTime;
    public float speed;
    public TextMeshProUGUI namePlayer;
    private float getSpeed;

    private Transform transformPlayer;

    private Vector2 getTransform;
    [HideInInspector] public PhotonView view;

    public Transform canvasTransformPlayer;

    private bool isDead;

    private bool usingMagic;

    private bool wallColision;

    private float currentHearth;
    public float maxHearth;
    private float getMaxHearth;

    private bool protection;
    public Color[] color;

    private bool activeRegenerationHearth;

    private bool visible;//serve para verificar se o player está invisivel

    private int SelectedHability;
    private Vector2 getVector2;

    public AudioClip[] audioClip;

    private int RandomPlayer;

    public bool WallColision
    {
        get { return wallColision; }
        set { wallColision = value; }
    }

    private void Awake()
    {
        RandomPlayer = Random.Range(0, 2);
        directDistance = 0.9f;
        visible = true;
        getMaxHearth = maxHearth;
        getSpeed = speed;
        transformPlayer = GetComponent<Transform>();
        getTransform = new Vector2(transformPlayer.position.x, transformPlayer.position.y);
        getResurrectionTime = resurrectionTime;

        rigidbody2D = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        if (view.IsMine)
        {
            view.RPC("SetAnimator", RpcTarget.AllBuffered, "SelectPlayer", RandomPlayer);
            view.RPC("SetSpriteRendererIcon", RpcTarget.AllBuffered, RandomPlayer);
        }
    }

    private void Start()
    {
        if (view.IsMine)
        {
            SetNamePlayer();
        }
        SetBar(getMaxHearth);
    }

    private void FixedUpdate()
    {
        if (!isDead && !usingMagic)
        {
            Movie();
        }

    }

    public void SetNamePlayer()
    {
        view.RPC("SetNamePlayerRPC", RpcTarget.AllBuffered, PhotonNetwork.NickName);
    }

    // Update is called once per frame
    void Update()
    {
        //a barra de Hp do Player apenas será mostrado se o valor da vida for abaixo de 100%
        if (currentHearth == maxHearth || currentHearth == 0)
        {
            bar.SetActive(false);
        }
        else
        {
            bar.SetActive(true);
        }

        //Selecionando os Botões do painel para selecionar as habilidades
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Caso esteja invisível, volta a ficar visível depois de levar dano.
            if (!visible)
            {
                SetInvisible();
            }
            PontosController.instance.SetIconPainel(0);
            SelectedHability = 0;
            SetFalseMagic();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Caso esteja invisível, volta a ficar visível depois de levar dano.
            if (!visible)
            {
                SetInvisible();
            }
            PontosController.instance.SetIconPainel(1);
            SelectedHability = 1;
            SetFalseMagic();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Caso esteja invisível, volta a ficar visível depois de levar dano.
            if (!visible)
            {
                SetInvisible();
            }
            PontosController.instance.SetIconPainel(2);
            SelectedHability = 2;
            SetFalseMagic();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PontosController.instance.SetIconPainel(3);
            SelectedHability = 3;
            SetFalseMagic();
        }


        //Desativa o particula de coração do personagem
        if (currentHearth == maxHearth)
        {
            particleSystem.Stop();
        }



        if (view.IsMine && !isDead && !usingMagic)
        {
            if (!protection)
            {
                OnInput();
            }


            if (direction.sqrMagnitude > 0)
            {
                AnimatorActiveOnceInt("direction", 1);
            }
            else
            {
                AnimatorActiveOnceInt("direction", 0);
            }

            if (direction.x > 0)
            {
                direct = 0;
                directDistance = 0.9f;
                transform.eulerAngles = new Vector2(0, 0);
                bar.transform.eulerAngles = new Vector2(0, 0);
                namePlayer.transform.eulerAngles = new Vector2(0, 0);

            }

            if (direction.x < 0)
            {
                direct = 1;
                directDistance = -0.9f;
                transform.eulerAngles = new Vector2(0, 180);
                bar.transform.eulerAngles = new Vector2(0, 0);
                namePlayer.transform.eulerAngles = new Vector2(0, 0);


            }

            if (!wallColision)
            {
                if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space)) && !usingMagic)
                {

                    if (SelectedHability == 0)
                    {
                        if (!visible)
                        {
                            SetInvisible();
                        }
                        usingMagic = true;
                        AnimatorActiveOnceInt("direction", 2);
                    }
                    else if (SelectedHability == 1)
                    {
                        if (!visible)
                        {
                            SetInvisible();
                        }
                        usingMagic = true;
                        AnimatorActiveOnceInt("direction", 3);
                    }
                    else if (SelectedHability == 3)
                    {
                        usingMagic = true;
                        animator.SetTrigger("visible");
                    }

                }

                if (SelectedHability == 2 && animator.GetInteger("direction") == 0)
                {
                    if (!visible)
                    {
                        SetInvisible();
                    }

                    if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Space))
                    {
                        AnimatorActiveOnceInt("direction", 4);
                        speed = 0;
                        protection = true;
                    }

                    if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Space))
                    {
                        AnimatorActiveOnceInt("direction", 0);
                        speed = getSpeed;
                        protection = false;
                    }
                }
            }


            //Ativa a Regeneração do personagem
            if (activeRegenerationHearth)
            {
                if (currentHearth < maxHearth)
                {
                    currentHearth += Time.deltaTime;
                    SetFillAmout();
                }

                if (currentHearth >= maxHearth)
                {
                    currentHearth = maxHearth;
                    activeRegenerationHearth = false;
                }
            }
        }

        //é ativado com personagem morrer
        if (isDead)
        {
            resurrectionTime -= Time.deltaTime;
            if (resurrectionTime <= 0)
            {
                ResetPlayer();//reseta todos os atributos do player
            }
        }
    }

    public void SetAudio(int value)
    {
        ActorSFX.instance.PlaySfx(audioClip[value]);
    }

    public void SetAudioOnce(int value)
    {
        ActorSFX.instance.PlaySfxOnce(audioClip[value]);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Axe") || other.CompareTag("Magic") || other.CompareTag("Dano")) && !isDead && !protection)
        {
            if (view.IsMine)
            {
                //Caso esteja invisível, volta a ficar visível depois de levar dano.
                if (!visible)
                {
                    SetInvisible();
                }

                if (other.CompareTag("Axe"))
                {
                    RemoveBar(2);
                }
                else if (other.CompareTag("Magic"))
                {
                    RemoveBar(5);
                }
                else if (other.CompareTag("Dano"))
                {
                    RemoveBar(1);
                }

                SofrerDano();
                // view.RPC("SofrerDanoRPC", RpcTarget.OthersBuffered);
                if (currentHearth <= 0)
                {
                    SetPlayer();
                    IsDeadPlayer();
                }
            }
        }

        if (other.CompareTag("Colisor"))
        {
            wallColision = true;
            if (currentHearth < maxHearth)
            {
                particleSystem.Play();
                activeRegenerationHearth = true;
            }

        }
    }

    public void SetPlayer()
    {
        view.RPC("SetAnimator", RpcTarget.AllBuffered, "SelectPlayer", RandomPlayer);
        view.RPC("SetAnimator", RpcTarget.AllBuffered, "direction", -1);
    }

    private void SofrerDano()
    {
        animator.SetTrigger("hit");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Colisor"))
        {
            wallColision = false;
            particleSystem.Stop();
            activeRegenerationHearth = false;
        }
    }

    public void RemoveBar(float value)
    {
        currentHearth -= value;
        SetFillAmout();
    }

    public bool GetisDead()
    {
        return isDead;
    }

    public void SetBar(float value)
    {
        currentHearth = value;
        SetFillAmout();
    }

    public void Addmagic()
    {
        if (view.IsMine)
        {
            getVector2 = new Vector2(transform.position.x + directDistance, transform.position.y);
        }
        if (!isDead && usingMagic)
        {
            PainelObjects.instance.SetInMagic(getVector2, direct);

            SetFalseMagic();
        }
    }

    public void SetColorPlayerRPC(int value)
    {
        view.RPC("ColorPlayer", RpcTarget.AllBuffered, value, visible);
    }

    public void SetColorPlayer(int value)
    {
        ColorPlayer(value, true);
    }

    public void SetInvisible()
    {
        if (visible)
        {
            visible = false;
            SetColorPlayerRPC(1);
            SetColorPlayer(2);
        }
        else
        {
            visible = true;
            SetColorPlayerRPC(0);
            SetColorPlayer(0);

        }

        usingMagic = false;
    }

    public void SetFalseMagic()
    {
        AnimatorActiveOnceInt("direction", 0);
        StartCoroutine(WaitTime(0.1f));
    }

    public bool GetVisible()
    {
        return visible;
    }

    private void Movie()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + direction * speed * Time.deltaTime);
    }

    private IEnumerator WaitTime(float value)
    {
        yield return new WaitForSeconds(value);
        usingMagic = false;
        // AnimatorActiveOnceInt("direction", 0);
    }

    void OnInput()
    {
        //Direção do Personagem
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public void AddPoint(int qtn, int pos)
    {
        if (view.IsMine)
        {
            view.RPC("AddPointForAll", RpcTarget.AllBuffered, qtn, pos);
        }

    }

    private void AnimatorActiveOnceInt(string anin, int value)
    {
        if (animator.GetInteger(anin) != value)
        {
            animator.SetInteger(anin, value);
        }
    }

    //quando o Plyer morre
    private void IsDeadPlayer()
    {
        resurrectionTime = getResurrectionTime;
        isDead = true;

        view.RPC("DropItemDead", RpcTarget.AllBuffered);

    }

    public void SetFillAmout()
    {
        view.RPC("TotalFillAmout", RpcTarget.AllBuffered);
    }

    private void ResetPlayer()
    {
        SetBar(1);
        isDead = false;
        AnimatorActiveOnceInt("direction", 0);
        transform.position = getTransform;
        speed = getSpeed;
        usingMagic = false;
        SetColorPlayerRPC(0);
        SetColorPlayer(0);
        visible = true;

        //Caso esteja invisível, volta a ficar visível depois de levar dano.
        if (!visible)
        {
            SetInvisible();
        }
    }

    #region PunRPC

    [PunRPC]
    public void SetNamePlayerRPC(string nome)
    {
        namePlayer.text = nome;
    }

    [PunRPC]
    public void DropItemDead()
    {
        float posY = 1f;
        float posX = 0.5f;
        Vector2 vector2 = new Vector2(transform.position.x + (posX + 0.3f), transform.position.y - (posY + 0.3f));

        int value;

        for (int i = 0; i < SpawnUI.instance.GetPointQtn(); i++)
        {
            value = SpawnUI.instance.GetPoint(i);
            if (value == 0)
            {
                continue;
            }

            // getSpriteDrops = SpawnUI.instance.GetPoint(i);
            ControlerColect.instance.DropColectable(vector2, value, i, i);
            SpawnUI.instance.Resetpainel(0, i);
        }
    }

    [PunRPC]
    private void SofrerDanoRPC()
    {
        animator.SetTrigger("hit");
    }

    [PunRPC]
    public void SetSpriteRendererIcon(int value)
    {
        if (spriteRendererIcon.sprite != icons[value])
        {
            spriteRendererIcon.sprite = icons[value];
        }
    }

    [PunRPC]
    private void ColorPlayer(int value, bool active = true)
    {
        spriteRenderer.color = color[value];
        bar.GetComponent<Image>().color = color[value];
        barFillAmount.color = color[value];
        spriteRendererIcon.color = color[value];
        barName.SetActive(active);
    }
    [PunRPC]
    private void TotalFillAmout()
    {
        if (view.IsMine)
        {
            barFillAmount.fillAmount = currentHearth / maxHearth;
        }
    }

    [PunRPC]
    public void AddPointForAll(int qtn, int pos)
    {
        if (view.IsMine)
        {
            // Acesse o painel do jogador e adicione pontos
            SpawnUI.instance.Setpainel2(qtn, pos);
        }
    }

    [PunRPC]
    public void SetColor(int value)
    {
        spriteRenderer.color = SpawnPlayers.instance.colors[value];
    }

    [PunRPC]
    public void SetAnimator(string nome, int value)
    {
        animator.SetInteger(nome, value);
    }

    #endregion




}
