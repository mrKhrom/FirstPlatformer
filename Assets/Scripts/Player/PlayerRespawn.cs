using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound; //Sound that we will play when we reach a checkpoint
    private Transform currentCheckpoint; // We will store our last checkpoint here
    private Health playerHealth; //player health when he made a checkpoint

    private void Awake()
    {
        playerHealth = GetComponent<Health>(); 
    }

    private void respawn()
    {
        transform.position = currentCheckpoint.position; //Move player to the last checkpoint
        playerHealth.Respawn(); //Restore player's health
    
        //Move camera to the player position
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    //Activate checkpoint when player enters it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; //Set current checkpoint to the one we just entered
            SoundManager.instance.PlaySound(checkpointSound); //Play checkpoint sound
            collision.GetComponent<Collider2D>().enabled = false; //Disable checkpoint collider so we can't activate it again
            collision.GetComponent<Animator>().SetTrigger("appear"); //Play checkpoint animation
        }
    }
}
