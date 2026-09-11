using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public enum State { Orbiting, Attacking }
    [Header("Target/physics")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rb;
    [Header("Orbiting")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float orbitRadius = 5f;
    [SerializeField] private float minDistanceToPlayer = 2f;
    [SerializeField] private float waypointTolerance = 0.5f;
    [SerializeField] private float timeBetweenWaypoints = 2f;
    [Header("Attack")]
    [SerializeField] private float attackCooldown = 4f;
    [SerializeField] private float attackSpeed = 12f;
    [SerializeField] private float attackDuration = 0.5f;

    private State currentState = State.Orbiting;
    private Vector2 currentTargetPoint;
    private float waypointTimer;
    private float attackCooldownTimer;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        // Generate the first random movement target
        currentTargetPoint = GetRandomPointAroundPlayer();
        attackCooldownTimer = attackCooldown;
    }

    void Update()
    {
        // Handle timers only while orbiting
        if (currentState == State.Orbiting)
        {
            waypointTimer -= Time.deltaTime;
            attackCooldownTimer -= Time.deltaTime;

            // Trigger an attack if the cooldown is over
            if (attackCooldownTimer <= 0f)
            {
                StartCoroutine(AttackRoutine());
            }
            // Otherwise, pick a new random spot if enough time passed or if we reached the current spot
            else if (waypointTimer <= 0f || Vector2.Distance(transform.position, currentTargetPoint) < waypointTolerance)
            {
                currentTargetPoint = GetRandomPointAroundPlayer(); //gets random point and resets the timer
                waypointTimer = timeBetweenWaypoints;
            }
        }
    }

    void FixedUpdate()
    {
        //moves enemy randomly 
        if (currentState == State.Orbiting)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, currentTargetPoint, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
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

        float elapsedTime = 0f;
        while (elapsedTime < attackDuration)
        {
            //Enemy dashes like hell into player
            rb.linearVelocity = attackDirection * attackSpeed;
            elapsedTime += Time.deltaTime;
            yield return null;
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

}
