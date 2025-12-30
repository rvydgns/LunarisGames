using UnityEngine;

public class LioraTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;

       
        if (GameManager.instance.lioraCutscenePlayed)
            return;

        if (triggered)
            return;

        triggered = true;

        if (Level2AudioController.instance != null)
            Level2AudioController.instance.EnterCutsceneAudio();

       
        CutsceneManager.instance.StartLioraCutscene();
    }
}
