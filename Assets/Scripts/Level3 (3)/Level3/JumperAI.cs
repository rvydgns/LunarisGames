using UnityEngine;

public class JumperAI : MonoBehaviour
{
    public float jumpHeight = 2.5f;
    public float jumpDuration = 0.4f;
    public float jumpInterval = 2f;

    private Vector2 startPos;
    private float timer;
    private bool isJumping;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isJumping && timer >= jumpInterval)
        {
            timer = 0f;
            StartCoroutine(Jump());
        }
    }

    System.Collections.IEnumerator Jump()
    {
        isJumping = true;

        float t = 0f;
        while (t < 1f)
        {
            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.position = new Vector2(startPos.x, startPos.y + yOffset);
            t += Time.deltaTime / jumpDuration;
            yield return null;
        }

        transform.position = startPos;
        isJumping = false;
    }
}
