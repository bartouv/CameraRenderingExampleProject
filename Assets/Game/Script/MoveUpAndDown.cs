using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    public float speed = 2.0f;
    public float amount = 2.0f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * speed) * amount, 0);
    }
}