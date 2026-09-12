using UnityEngine;
using System.Collections;

public class StoneBehavior : MonoBehaviour
{
    public enum State { Resting, Attacking }
    private SpriteRenderer enemySprite;
    private Rigidbody2D rb; 
    private Transform target;
    private bool canSpawnSlime;
    [SerializeField] private GameObject slimePrefab;
    [Header("Movement")]
    [SerializeField] private float speed = 7f; 
    [SerializeField] private float turnSpeed = 2f;//how rotatable enemy is
    [Header("Characteristics")]
    [SerializeField] private int damage = 1;
    [SerializeField] private int health = 3;
    [SerializeField] private float restTime = 10f;
    [SerializeField] private float slimeRTime;
    [SerializeField] private float maxSlimeRTime;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float MaxtimeBetweenAttacks;
    private State currentState = State.Attacking;
    private Vector2 currentMovement;
    private Transform cachedTransform;

    void Start()
    {
        cachedTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemySprite = GetComponent<SpriteRenderer>();
        timeBetweenAttacks = MaxtimeBetweenAttacks;
        slimeRTime = maxSlimeRTime;
    }

    void Update()
    {
        //if (PauseScript.isPaused) return;
        if (currentState == State.Attacking)
        {
            timeBetweenAttacks -= Time.deltaTime;
            slimeRTime -= Time.deltaTime;
            if (slimeRTime <= 0f) 
            {
                Destroy(Instantiate(slimePrefab, transform.position, transform.rotation),3f);
                slimeRTime = maxSlimeRTime;
            }
            if (timeBetweenAttacks <= 0f)
            {
                StartCoroutine(RestingRoutine());
            }
            Vector2 targetDirection = (target.transform.position - cachedTransform.position).normalized;
            // This creates the bad rotation
            currentMovement = Vector2.MoveTowards(currentMovement, targetDirection, turnSpeed * Time.deltaTime);
            AimMouse(currentMovement);
            
        }
    }

    private void FixedUpdate()
    {
        if (currentState == State.Attacking)
        {
            rb.MovePosition(rb.position + (currentMovement * speed * Time.fixedDeltaTime));
        }
    }
    private IEnumerator RestingRoutine()
    {
        currentState = State.Resting;
        yield return new WaitForSeconds(restTime);
        timeBetweenAttacks = MaxtimeBetweenAttacks;
        currentState = State.Attacking;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerMovement>(out PlayerMovement player))
            {
                player.takeDamage(damage);
            }

        }
        else if (collision.CompareTag("OurBullet"))
        {
            health--;
            if (health <= 0)
            {
                // TODO: Add points to player score manager here
                // TODO: Instantiate particle effect
                Destroy(gameObject);

            }
        }
    }

    void AimMouse(Vector2 direction)   //thing to rotate enemy,same used for weapon and player
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle > 90 || angle < -90)
        {
            enemySprite.flipX = false;
        }
        else
        {
            enemySprite.flipX = true;
        }
    }
}
