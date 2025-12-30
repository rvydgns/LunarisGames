using UnityEngine;

namespace ClearSky
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerRespawn : MonoBehaviour
    {
        [Header("Spawn Points")]
        public Transform playerStart;         
        public Transform currentCheckpoint;   

        [Header("Fall Death")]
        public bool enableFallDeath = true;
        public float deathY = -20f;

        [Header("Respawn Settings")]
        public bool restoreFullHealthOnRespawn = true;

        private Rigidbody2D rb;
        private Animator anim;
        private PlayerMovement movement;
        private PlayerHealth health;

        private bool isRespawning = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            movement = GetComponent<PlayerMovement>();
            health = GetComponent<PlayerHealth>();
        }

        private void Start()
        {
           
            if (playerStart == null)
            {
                GameObject spawn = GameObject.Find("SpawnPoint");
                if (spawn != null)
                    playerStart = spawn.transform;
            }

          
            if (currentCheckpoint == null)
                currentCheckpoint = playerStart;

           
            if (currentCheckpoint != null)
                transform.position = currentCheckpoint.position;
        }

        private void Update()
        {
            if (!enableFallDeath || isRespawning) return;

            
            if (transform.position.y < deathY)
            {
                Respawn();
            }
        }

        
        public void SetCheckpoint(Transform checkpoint)
        {
            currentCheckpoint = checkpoint;
            Debug.Log("Checkpoint set: " + checkpoint.name);
        }

       
        public void Respawn()
        {
            if (isRespawning) return;

            Transform spawnPoint = currentCheckpoint != null
                ? currentCheckpoint
                : playerStart;

            if (spawnPoint == null)
            {
                Debug.LogWarning("Respawn failed: No spawn point.");
                return;
            }

            isRespawning = true;

           
            if (movement != null)
                movement.enabled = false;

          
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.rotation = 0f;

            
            transform.position = spawnPoint.position;

            
            if (restoreFullHealthOnRespawn && health != null)
                health.RestoreFullHealth();

           
            anim.Rebind();
            anim.Update(0f);
            anim.Play("Idle");

          
            if (movement != null)
                movement.enabled = true;

            isRespawning = false;
        }
    }
}
