using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    float fireTimer = 0f;
    float iceTimer;
    [SerializeField]float elementalTimerLength;
    float fireDamageCooldown = 1f;
    [SerializeField] SpriteRenderer enemySprite;
    public GameObject target;
    [SerializeField] float speed;
    float savedSpeed;
    [SerializeField] int damage,health;
    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerMovement player;
    public Weapon gun;
    private float elementalLevel;
    [SerializeField] int pointsToGive, moneyToGive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player");
        player = Object.FindAnyObjectByType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        savedSpeed = speed;
        iceTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 direction = target.transform.position - transform.position;//calculate where enemy need to go
        AimMouse(direction);
        direction.Normalize();
        movement = direction.normalized;
        if(fireTimer >= 0)
        {
            if(fireDamageCooldown <= 0)
            {
                doFireDamage();
                fireDamageCooldown = 0.5f;
            }
            fireDamageCooldown-=Time.deltaTime;

            fireTimer-=Time.deltaTime;
        }
        if(iceTimer > 0)
        {
            speed= 0.5f * savedSpeed;
            Debug.Log("THEY SLOWED");
            iceTimer-=Time.deltaTime;
        }
        else
        {
            speed = savedSpeed;
        }


    }
    private void FixedUpdate()
    {
        moveEnemy(movement);
    }
    void moveEnemy(Vector2 movement)
    {
        //rb.MovePosition((Vector2)rb.position + (movement * speed * Time.deltaTime)); //The calculation for making enemy move
        rb.linearVelocity = movement * speed * Time.deltaTime;
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
            BulletsScript bulletScript = collision.GetComponent<BulletsScript>();
            
            health -= 1 + bulletScript.damageLevel;
            Debug.Log("WE REACHED ELEMENTAL SHIT");
            switch (gun.currentElement.ToString())
            {

                case "ice":
                    elementalTimerLength = 0.5f * gun.elementalLevel;
                    iceTimer = elementalTimerLength;
                    elementalLevel = gun.elementalLevel;
                    break;
                case "fire":
                    elementalTimerLength = 0.5f * gun.elementalLevel;
                    fireTimer = elementalTimerLength;
                    elementalLevel = gun.elementalLevel;
                    break;
                case "wind":
                    doKnockback(gun.knockbackStrength, gun.direction);
                    elementalLevel = gun.elementalLevel;
                    break;
            }
            if(health <= 0)
            {
                player.rewardForKill(moneyToGive, pointsToGive);
                //particle system or smth
                //+points for player
                Destroy(gameObject);
            }
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("OurSword"))
        {
            SwordScript swordScript = collision.GetComponent<SwordScript>();
            health-= 2 + swordScript.damageLevel;
            
            switch (swordScript.currentElement.ToString())
            {

                 case "ice":
                    elementalTimerLength = 0.5f * swordScript.elementalLevel;
                    iceTimer = elementalTimerLength;
                    elementalLevel = swordScript.elementalLevel;
                    break;
                case "fire":
                    elementalTimerLength = 0.5f * swordScript.elementalLevel;
                    fireTimer = elementalTimerLength;
                    elementalLevel = swordScript.elementalLevel;
                    break;
                case "wind":
                    doKnockback(swordScript.knockbackStrength, swordScript.direction);
                    elementalLevel = swordScript.elementalLevel;
                    break;
            }
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
    void doKnockback(float knockBackStrength, Vector2 direction)
    {
        rb.AddForce(direction.normalized * knockBackStrength * 6000, ForceMode2D.Force);
        Debug.Log("KNOCKBACK DONE");
    }
    void doFireDamage()
    {
        health -= 1;
        if(health <= 0)
            {
                player.rewardForKill(moneyToGive, pointsToGive);
                //particle system or smth
                //+points for player
                Destroy(gameObject);
            }
        Debug.Log("fire damage taken");
    }
    // async void doSlowDown()
    // {
    //     float savedSpeed = speed;
    //     speed = speed * 0.5f;
    //     await Awaitable.WaitForSecondsAsync(5f);

    // }
}
