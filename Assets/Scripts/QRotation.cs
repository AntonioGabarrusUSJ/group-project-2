using UnityEngine;

public class QRotation : MonoBehaviour
{
    public float Speed = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion qRotation = Quaternion.AngleAxis(Time.deltaTime * Speed,Vector3.up);

        transform.localRotation *= qRotation;
    }
}
