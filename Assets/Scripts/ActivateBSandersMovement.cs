using UnityEngine;

public class ActivateBSandersMovement : MonoBehaviour
{
    public GameObject BSanders;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BSanders.GetComponent<MoveBSanders>().enabled = true;
        }
    }
}
