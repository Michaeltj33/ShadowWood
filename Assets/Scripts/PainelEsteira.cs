using UnityEngine;

public class PainelEsteira : MonoBehaviour
{
    public GameObject madeiraGameObject;

    private Vector2 vector2Madeira;

    private bool coliderPlayer;//arrumar

    private bool active;

    public float speed;
    // Start is called before the first frame update
    void Awake()
    {
        madeiraGameObject.GetComponent<ColectableMadeira>().SetActiveObject(false);
        active = false;
        madeiraGameObject.SetActive(false);
        vector2Madeira = new Vector2(madeiraGameObject.transform.position.x, madeiraGameObject.transform.position.y);
    }


    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            madeiraGameObject.transform.Translate(new Vector2(0, speed * Time.deltaTime));
        }


        if (Input.GetKeyDown(KeyCode.E) && coliderPlayer)
        {
            SetMadeira();
        }
    }

    public void SetMadeira()
    {
        madeiraGameObject.GetComponent<ColectableMadeira>().SetActiveObject(true);
        madeiraGameObject.transform.position = vector2Madeira;
        active = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coliderPlayer = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coliderPlayer = false;

        }
    }


}
