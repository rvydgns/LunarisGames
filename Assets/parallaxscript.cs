using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float speed = 0.1f;
    private Transform cam;
    private Vector3 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }

    void Update()
    {
        Vector3 delta = cam.position - lastCamPos;
        transform.position += new Vector3(delta.x * speed, delta.y * speed, 0);
        lastCamPos = cam.position;
    }
}
