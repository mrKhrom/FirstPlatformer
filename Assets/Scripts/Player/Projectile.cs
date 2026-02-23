using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifeTime;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(new Vector3(movementSpeed, 0, 0), Space.World);

        lifeTime += Time.deltaTime;
        if (lifeTime > 5) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
    }

    public void SetDirection(float _direction)
    {
        lifeTime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        // ensure projectile is not parented to player (or any other object)
        transform.SetParent(null);

        // use SpriteRenderer.flipX to mirror the sprite/animation instead of negative scale
        float scaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        if (spriteRenderer != null)
            spriteRenderer.flipX = _direction < 0;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

}