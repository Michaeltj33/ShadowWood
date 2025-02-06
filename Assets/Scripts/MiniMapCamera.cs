using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public static MiniMapCamera instance;

    private Transform TransformPlayer;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (TransformPlayer != null)
        {
            transform.position = new Vector3(TransformPlayer.position.x, transform.position.y, transform.position.z);
        }
    }

    public void SetCamera(Transform transform)
    {
        TransformPlayer = transform;
    }
}
