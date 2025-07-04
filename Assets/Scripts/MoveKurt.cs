using UnityEngine;

public class Mover : MonoBehaviour
{
    public GameObject BSanders;
    private Vector3 finalPosition;
    public float speed = 4.0f;
    private bool triggerBSanders = true;

    void Start()
    {
        finalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 19.22f);
        //BSanders.GetComponent<MoveBSanders>().enabled = false;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalPosition, speed * Time.deltaTime);
        if (triggerBSanders && transform.position == finalPosition)
        {
            triggerBSanders = false;
            BSanders.GetComponent <MoveBSanders>().enabled = true;
            
        }
    }
}
