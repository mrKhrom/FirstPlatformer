using UnityEngine;

public class SpikeHead : EnemyDamage
{
    [Header("SpikeHead Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private Vector3[] directions = new Vector3[4];
    private Vector3 destination;
    private float checkTimer;
    private bool attacking;


    private void OnEnable()
    {
        Stop();    
    }

    private void Update()
    {
        //Move spikehead to destination only if attacking
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= checkDelay)
                CheckForPlayer();
        }
    }
    private void CheckForPlayer()
    {
        CalculateDestination();
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);
            
            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i].normalized;
                checkTimer = 0;
            }    
        }    
    }
    private void CalculateDestination()
    {
        directions[0] =  transform.right * range; //Right
        directions[1] = -transform.right * range; //Left
        directions[2] =  transform.up * range; //Up
        directions[3] = -transform.up * range; //Down
    }

    private void Stop()
    {
        destination = transform.position; //Set destination as current position so it doesnt move
        attacking = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        //Stop spikehead if it hits something
        Stop();
    }
}
