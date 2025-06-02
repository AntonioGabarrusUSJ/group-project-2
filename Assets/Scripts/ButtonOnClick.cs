using UnityEngine;
using Unity.VisualScripting;

public class ButtonOnClick : MonoBehaviour
{
    public GameObject kurtPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(kurtPrefab == null) throw new System.Exception("Kurt Prefab not defined");
        //kurtPrefab.GetComponent<Mover>().enabled = false;
    }

    private void OnMouseDown()
    {
        kurtPrefab.GetComponent<Mover>().enabled = true;
    }
}
