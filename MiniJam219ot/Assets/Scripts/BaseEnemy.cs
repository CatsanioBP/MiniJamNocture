using UnityEngine;
using UnityEngine.LightTransport;

public class BaseEnemy : MonoBehaviour
{
    public Transform target;
    [SerializeField] float speed;
    [SerializeField] int damage;
    private Rigidbody2D rb;
    private Vector2 movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
          rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = target.position - transform.position;//calculate where enemy need to go
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
            PlayerMovement player = Object.FindAnyObjectByType<PlayerMovement>();//find object with player script so you can call take damage of player
            player.takeDamage(damage);//call function
            Destroy(gameObject);
        }   
    }
}
