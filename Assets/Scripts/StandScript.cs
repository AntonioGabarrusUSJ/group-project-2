using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class StandScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Car Stand objects")]
    public GameObject CarPrefab;
    public GameObject Cloth;

    private GameObject UsedCarPrefab;
    private GameObject UsedCloth;

    private GameObject CarInstance;
    private GameObject ClothInstance;

    private float renderingHeight = 0.7f;
    private float ClothHeight = 3f;
    void Start()
    {

        CleanScene();

        if (CarPrefab == null || Cloth == null)
        {
            UsedCarPrefab = null;
            UsedCloth = null;
            return;
        }
        this.UsedCarPrefab = CarPrefab;
        this.UsedCloth = Cloth;
        this.CarInstance = Instantiate(UsedCarPrefab, 
            new Vector3(this.transform.position.x, renderingHeight + this.transform.position.y, this.transform.position.z), 
            Quaternion.identity);
        this.CarInstance.transform.parent = this.transform;
        this.ClothInstance = Instantiate(UsedCloth, 
            new Vector3(this.transform.position.x, renderingHeight + ClothHeight + this.transform.position.y, this.transform.position.z), 
            Quaternion.identity);
        this.ClothInstance.transform.parent = this.transform;
        CapsuleCollider[] capsuleCollidersArray = this.ClothInstance.GetComponent<Cloth>().capsuleColliders;
        string capsuleCollidersArrayStringDescription = "Before: [ ";
        foreach (var capsuleCollider in capsuleCollidersArray)
        {
            capsuleCollidersArrayStringDescription += $"{((capsuleCollider == null) ? "null" : capsuleCollider.GetHashCode())} ";
        }
        capsuleCollidersArrayStringDescription += "]\nAfter: [ ";
        capsuleCollidersArray = new CapsuleCollider[1];
        capsuleCollidersArray[0] = this.CarInstance.GetComponent<CapsuleCollider>();
        this.ClothInstance.GetComponent<Cloth>().capsuleColliders = capsuleCollidersArray;
        foreach (var capsuleCollider in capsuleCollidersArray)
        {
            capsuleCollidersArrayStringDescription += $"{((capsuleCollider == null) ? "null" : capsuleCollider.GetHashCode())} ";
        }
        capsuleCollidersArrayStringDescription += "]";
        Debug.Log(capsuleCollidersArrayStringDescription);
    }

    // Update is called once per frame
    void Update()
    {
        // If any parameter has changed, we reinitialize the component (for the edit interface to be rendered correctly)
        if (this.UsedCarPrefab != CarPrefab || this.Cloth != this.UsedCloth)
        {
            CleanScene();
            Start();
        }

        // If any parameter is null, nothing has to be re-rendered
        if(this.UsedCloth == null || this.UsedCarPrefab == null)
        {
            return;
        }

        Debug.Log($"Selected Car prefab: {CarPrefab.GetHashCode()}\n" +
            $"Using Car prefab: {UsedCarPrefab.GetHashCode()}\n" +
            $"Selected Cloth prefab: {Cloth.GetHashCode()}\n" +
            $"Using Cloth prefab: {UsedCloth.GetHashCode()}");

    }

    void CleanScene()
    {
        if (this.CarInstance != null && this.CarInstance.IsDestroyed())
        {
            Destroy(this.CarInstance);
        }
        if (this.ClothInstance != null && this.ClothInstance.IsDestroyed())
        {
            Destroy(this.ClothInstance);
        }
    }
}
