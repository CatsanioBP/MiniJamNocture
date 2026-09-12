using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public enum State { Orbiting, Attacking }
    [Header("Target/physics")]
    private PlayerMovement player;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rb;
    [Header("Orbiting")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float orbitRadius = 5f;//the orbit where point is choosen
    [SerializeField] private float minDistanceToPlayer = 2f;  //limit of orbit radius basicly,you can't spawn points in there
    [SerializeField] private float waypointTolerance = 0.5f;
    [SerializeField] private float timeBetweenWaypoints = 2f;
    [Header("Attack")]
    [SerializeField] private float attackCooldown = 4f;
    [SerializeField] private float attackSpeed = 12f;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private bool isMelee;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private SpriteRenderer enemySprite;
    [Header("Stats")]
    [SerializeField] private int damage = 1;
    [SerializeField] private int health = 3;
    [SerializeField] int pointsToGive, moneyToGive;

    private State currentState = State.Orbiting;
    private Vector2 currentTargetPoint;
    private float waypointTimer;
    private float attackCooldownTimer;

    void Start()
    {
        player = Object.FindAnyObjectByType<PlayerMovement>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        // Generate the first random movement target
        currentTargetPoint = GetRandomPointAroundPlayer();
        attackCooldownTimer = attackCooldown;
        waypointTimer = timeBetweenWaypoints; 
    }

    void Update()
    {
        if (currentState == State.Orbiting)
        {                                  
            waypointTimer -= Time.deltaTime;
            attackCooldownTimer -= Time.deltaTime;

            if (attackCooldownTimer <= 0f)
            {
                StartCoroutine(AttackRoutine());
            }
        }
    }

    void FixedUpdate()
    {
        //moves enemy randomly 
        if (currentState == State.Orbiting)
        {
            HandleOrbitingMovement();
        }
    }

    private void HandleOrbitingMovement()
    {
        Vector2 targetDirection = (currentTargetPoint - rb.position).normalized;
        Vector2 desiredVelocity = targetDirection * moveSpeed;
        AimMouse(targetDirection);

        //chech distance to player
        Vector2 vectorToPlayer = (Vector2)playerTransform.position - rb.position;
        float distanceToPlayer = vectorToPlayer.magnitude;

        //apply a push away from the player
        if (distanceToPlayer < minDistanceToPlayer)
        {
            Vector2 escapeDirection = -vectorToPlayer.normalized;
            //closer the enemy is, the harder it gets pushed away
            float pushStrength = (minDistanceToPlayer - distanceToPlayer) / minDistanceToPlayer;
            desiredVelocity += escapeDirection * (moveSpeed * 2f) * pushStrength;
        }

        //apply the position
        Vector2 newPosT = rb.position + desiredVelocity * Time.fixedDeltaTime;
        rb.MovePosition(newPosT);

        float distanceToTarget = Vector2.Distance(rb.position, currentTargetPoint);
        if (distanceToTarget <= waypointTolerance || waypointTimer <= 0f)
        {
            currentTargetPoint = GetRandomPointAroundPlayer();
            waypointTimer = timeBetweenWaypoints;
        }
    }

    private Vector2 GetRandomPointAroundPlayer()
    {
        if (playerTransform == null) return transform.position;

        // Generate a random vector inside a circular area
        Vector2 randomOffset = Random.insideUnitCircle * orbitRadius;

        // Prevent the point from being spawn exactly on top of the player
        if (randomOffset.magnitude < minDistanceToPlayer)
        {
            randomOffset = randomOffset.normalized * minDistanceToPlayer;
        }

        return (Vector2)playerTransform.position + randomOffset;
    }

    private IEnumerator AttackRoutine()
    {
        currentState = State.Attacking;

        // just things to find player pos to dash into him
        Vector2 attackTarget = playerTransform.position;
        Vector2 attackDirection = (attackTarget - rb.position).normalized;
        //sAimMouse(attackTarget);

        if (isMelee)
        {
            //Enemy dashes like hell into player
            float elapsedTime = 0f;
            while (elapsedTime < attackDuration)
            {
                rb.linearVelocity = attackDirection * attackSpeed;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            if (bulletPrefab != null && firePoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                // Calculate direction towards the player
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>(); 
                if (bulletRb != null)
                {
                    float bulletSpeed = 10f;
                    bulletRb.linearVelocity = attackDirection * bulletSpeed; 
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

        // it's kinda resets things and make it back to ordiniting around player
        rb.linearVelocity = Vector2.zero;
        attackCooldownTimer = attackCooldown;
        currentTargetPoint = GetRandomPointAroundPlayer();
        waypointTimer = timeBetweenWaypoints;

        currentState = State.Orbiting;
    }

    //Draw gizmo is for myself to see how things works exacly + easier to understand
    void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerTransform.position, orbitRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerTransform.position, minDistanceToPlayer);
        }
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
                player.rewardForKill(moneyToGive, pointsToGive);
                // TODO: Add points to player score manager here
                // TODO: Instantiate particle effect
                Destroy(gameObject);

            }
        }
        else if (collision.CompareTag("OurSword"))
        {
            health--;
            if (health <= 0)
            {
                player.rewardForKill(moneyToGive, pointsToGive);
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
