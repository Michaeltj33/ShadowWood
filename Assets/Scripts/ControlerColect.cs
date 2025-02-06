using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ControlerColect : MonoBehaviour
{
    ItemSorteio itemSorteio = new();
    public GameObject ApplePrafabs;

    public Sprite[] spriteDrops;

    public static ControlerColect instance;
    public int valueItem;
    public Vector2 waitingPosition;
    public SpawnPlayers spawnPlayers;
    private List<GameObject> gameObjectsTree = new();

    private PhotonView view;

    private void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InstantiateObjectsScene();
            itemSorteio.AddNewItem("0", 400);
            itemSorteio.AddNewItem("1", 300);
            itemSorteio.AddNewItem("2", 100);
            itemSorteio.AddNewItem("3", 80);
            itemSorteio.AddNewItem("4", 40);
            itemSorteio.AddNewItem("5", 30);
            itemSorteio.AddNewItem("10", 1);
        }
    }


    public int GetQuantityObject()
    {
        if (!PhotonNetwork.IsMasterClient)
            return 0;

        ItemSorteio.Item item;
        item = itemSorteio.Sortearitem();
        return int.Parse(item.nome);
    }

    private void InstantiateObjectsScene()
    {

        for (int i = 0; i < valueItem; i++)
        {
            view.RPC("InstantiateObjectsSceneRPC", RpcTarget.AllBuffered, ApplePrafabs.name, new Vector2(-77.15f, -56.95f));
        }

    }

    public void DropColectable(Vector2 vector2, int qtn, int valueIcon, int getSpriteDrops)
    {
        view.RPC("DropColectableRPC", RpcTarget.AllBuffered, vector2, qtn, valueIcon, getSpriteDrops);
    }

    private int VerifySorteioColor(int qtn)
    {
        if (qtn == 0)
        {
            return 0;
        }
        else if (qtn >= 1 && qtn <= 5)
        {
            return 1;
        }
        else if (qtn >= 6 && qtn <= 9)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    #region PunRPC
    [PunRPC]
    public void InstantiateObjectsSceneRPC(string name, Vector2 vector2)
    {

        GameObject getGameObject = PhotonNetwork.Instantiate(name, vector2, Quaternion.identity);
        getGameObject.SetActive(false);
        gameObjectsTree.Add(getGameObject);

    }

    [PunRPC]
    public void DropColectableRPC(Vector2 vector2, int qtnObject, int valueIcon, int getSpriteDrops)
    {
        bool verify = true;
        foreach (var obj in gameObjectsTree)
        {
            if (!obj.activeSelf)
            {
                int setColor;
                setColor = VerifySorteioColor(qtnObject);
                verify = false;
                obj.SetActive(true);
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = spriteDrops[valueIcon];
                obj.GetComponent<Animator>().SetBool("disabled", false);
                obj.GetComponent<Colectable>().SetItemDropTotal(qtnObject);
                obj.GetComponent<Colectable>().SetColorText(setColor);
                obj.GetComponent<Colectable>().getSpriteDrops = getSpriteDrops;
                obj.transform.position = vector2;
                break;
            }
        }

        if (!verify)
        {
            return;
        }
        else
        {
            view.RPC("InstantiateObjectsSceneRPC", RpcTarget.AllBuffered, ApplePrafabs.name, new Vector2(-77.15f, -56.95f));
            GameObject getGameObject = gameObjectsTree[gameObjectsTree.Count - 1];
            getGameObject.SetActive(true);
            getGameObject.transform.position = vector2;
        }
    }
    #endregion
}
