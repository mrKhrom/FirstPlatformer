using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [Header("Firetrap Timers@")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    [SerializeField] private float damageCooldown = 0.5f; // Cooldown between damage hits
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered; //when the trap gets triggerd
    private bool active; //when the trap is active and can hurt the player
    private float damageTimer; // Timer for damage cooldown

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
                StartCoroutine(ActivateFiretrap());
            if (active && damageTimer <= 0)
            {
                collision.GetComponent<Health>().TakeDamage(damage);
                damageTimer = damageCooldown;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && active && damageTimer <= 0)
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            damageTimer = damageCooldown;
        }
    }
    private IEnumerator ActivateFiretrap()
    {
        //turn the sprite red to notify the player and trigger the trap
        triggered = true;
        spriteRend.color = Color.red;

        //wait for delay, activate trap, turn on animation 
        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white; //turn the sprite rback to normal
        active = true;
        damageTimer = 0; // Reset damage timer when activating
        anim.SetBool("activated", true);

        //wait untill x seconds, deactivate trap and reset all variables and animator
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        damageTimer = 0; // Reset damage timer when deactivating
        anim.SetBool("activated", false);
    }  
}
