using UnityEngine;

//this component procedurally animates a mesh to bob up and down
public class MeshBob : MonoBehaviour
{
    //bobs per second
    [SerializeField] private float speed = 1f;
    [SerializeField] private float magnitude = 2f;
    [SerializeField] private GameObject mesh;

    private float initialY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialY = mesh.transform.position.y;
    }

    float GetBobYOffset()
    {
        return Mathf.Sin(Time.time * speed) * magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        mesh.transform.position = new Vector3(
            mesh.transform.position.x, 
            initialY + GetBobYOffset(), 
            mesh.transform.position.z
        );
    }
}
