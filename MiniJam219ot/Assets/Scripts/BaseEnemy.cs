using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] SpriteRenderer enemySprite;
    public GameObject target;
    [SerializeField] float speed;
    [SerializeField] int damage,health;
    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerMovement player;
    [SerializeField] int pointsToGive, moneyToGive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player");
        player = Object.FindAnyObjectByType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 direction = target.transform.position - transform.position;//calculate where enemy need to go
        AimMouse(direction);
        direction.Normalize();
        movement = direction.normalized;
    }
    private void FixedUpdate()
    {
        moveEnemy(movement);
    }
    void moveEnemy(Vector2 movement)
    {
        rb.MovePosition((Vector2)transform.position + (movement * speed * Time.deltaTime)); //The calculation for making enemy move
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //PlayerMovement player = Object.FindAnyObjectByType<PlayerMovement>();//find object with player script so you can call take damage of player
            player.takeDamage(damage);//call function
            Destroy(gameObject);
        }
        if(collision.CompareTag("OurBullet"))
        {
            health-= 1 + collision.GetComponent<BulletsScript>().damageLevel;
            if(health <= 0)
            {
                player.rewardForKill(moneyToGive, pointsToGive);
                //particle system or smth
                //+points for player
                Destroy(gameObject);
            }
        }
        if (collision.CompareTag("OurSword"))
        {
            health-= 2 + collision.GetComponent<SwordScript>().damageLevel;
            if(health <= 0)
            {
                player.rewardForKill(moneyToGive,pointsToGive);
                //particle system or smth
                //+points for player
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
