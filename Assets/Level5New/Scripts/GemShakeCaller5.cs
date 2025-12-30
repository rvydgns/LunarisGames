using UnityEngine;
using Cinemachine;

public class GemShakeCaller5 : MonoBehaviour
{
    public static CinemachineImpulseSource impulse;

    private void Awake()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
    }

    public static void TriggerShake()
    {
        if (impulse != null)
            impulse.GenerateImpulse();
    }
}
