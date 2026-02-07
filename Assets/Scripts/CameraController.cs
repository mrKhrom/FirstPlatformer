using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //Follw player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private float lookAhead;
    
    private void Update()
    {
        // Room camera
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);
    
        // Follow player with vertical clamp
        // float targetY = Mathf.Clamp(player.position.y, minY, maxY);
        // transform.position = new Vector3(player.position.x + lookAhead, targetY, transform.position.z);
        // lookAhead = Mathf.Lerp(lookAhead, aheadDistance * Input.GetAxis("Horizontal"), Time.deltaTime * cameraSpeed);    
    }

    public void MoveCamera(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }

}
