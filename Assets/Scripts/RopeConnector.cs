using Unity.VisualScripting;
using UnityEngine;

public class RopeConnector : MonoBehaviour
{
    [Header("Rope Connections")]
    public Rigidbody topConnectedBody;
    public Rigidbody bottomConnectedBody;

    [Header("Rope Definition")]
    [Min(0.1f)]
    public float lenght = 3f;

    [Range(0.05f, 0.5f)]
    public float ropeWidth;

    [Range(0.1f, 1f)]
    public float unitLength;
    
    public Material ropeMaterial = new Material(Shader.Find("Sprites/Default"));

    [Header("Optimizer")]
    public bool skipRenderFrames = true;

    [Range(1, 4)]
    public int frameCount = 3;

    private GameObject[] innerPoints;
    private LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        innerPoints = new GameObject[Mathf.CeilToInt(lenght / unitLength)];
        Vector3 actualPosiotion = transform.position + new Vector3(0f, lenght / 2, 0f);
        for (int i = 0; i < innerPoints.Length; ++i)
        {
            Debug.Log(i);
            innerPoints[i] = CreateInnerPoint(i,actualPosiotion);
            actualPosiotion -= new Vector3(0f,unitLength,0f);
        }

        GameObject lineContainer = new GameObject("Line");
        lineContainer.transform.position = transform.position;
        lineContainer.transform.parent = transform;
        lineRenderer = lineContainer.AddComponent<LineRenderer>();
        lineRenderer.material = ropeMaterial;
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        lineRenderer.positionCount = innerPoints.Length;
        lineRenderer.useWorldSpace = true;
    }


    GameObject CreateInnerPoint(int id, Vector3 position)
    {
        GameObject innerPoint = new GameObject($"Point_{id}");
        innerPoint.transform.position = position;
        innerPoint.transform.parent = transform;

        //RigidBody
        Rigidbody rb = innerPoint.AddComponent<Rigidbody>();
        rb.mass = 0.001f;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        
        //Collider
        BoxCollider collider = innerPoint.AddComponent<BoxCollider>();
        collider.center = Vector3.zero;
        collider.size = new Vector3(ropeWidth,unitLength, ropeWidth);
        collider.isTrigger = true;

        //Joint
        
        Rigidbody connectedBody = (id == 0) ? topConnectedBody : innerPoints[id - 1].GetComponent<Rigidbody>();
        SpringJoint joint = innerPoint.AddComponent<SpringJoint>();
        joint.connectedBody = connectedBody;
        joint.spring = 800f;
        joint.damper = 0.05f;
        joint.autoConfigureConnectedAnchor = true;
        joint.anchor = Vector3.zero + new Vector3(0f, unitLength, 0f);
        joint.axis = Vector3.zero;

        return innerPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if( skipRenderFrames && Time.frameCount % frameCount != 0) return;

        if (ropeMaterial == null)
        {
            ropeMaterial = new Material(Shader.Find("Sprites/Default"));
            ropeMaterial.color = Color.black;

        }
        for (int i = 0; i < innerPoints.Length; ++i)
        {
            lineRenderer.SetPosition(i, innerPoints[i].transform.position);
            lineRenderer.material = ropeMaterial;
        }

    }
}
