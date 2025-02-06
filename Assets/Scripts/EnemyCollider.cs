using UnityEngine;

public class EnemyCollider : MonoBehaviour
{

    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.VisiblePlayer = other.GetComponent<Player>().GetVisible();
            if (enemy.VisiblePlayer)
            {
                enemy.SetPlayer(other.GetComponent<Player>());
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.SetPlayer(null);
            enemy.VisiblePlayer = false;
        }
    }
}
