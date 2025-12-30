using Cinemachine;
using UnityEngine;

public class TorinCutsceneTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCutscene(other.gameObject);
        }
    }

    void StartCutscene(GameObject player)
    {
        Debug.Log("TORIN CUTSCENE START");

    
        var movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

       
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;

   
        var anim = player.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("isRun", false);
            anim.SetBool("isJump", false);
            anim.SetBool("isLookUp", false);

            anim.Play("Idle");
            anim.Update(0f);
        }

      
        var cutsceneCam = GameObject.Find("TorinCutsceneCam")
            .GetComponent<CinemachineVirtualCamera>();

        cutsceneCam.Priority = 20;

       
    }

    public void EndCutscene()
    {
        var cutsceneCam = GameObject.Find("TorinCutsceneCam")
            .GetComponent<CinemachineVirtualCamera>();

        cutsceneCam.Priority = 0;
    }
}
