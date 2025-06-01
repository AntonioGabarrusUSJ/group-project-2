using UnityEngine;

public class MoveBSanders : MonoBehaviour
{
    public Transform kurtTransform;
    private Vector3 finalPosition;
    public float speed = 3.0f;
    void Start()
    {
        finalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 19.05f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalPosition, speed * Time.deltaTime);
    }
}
