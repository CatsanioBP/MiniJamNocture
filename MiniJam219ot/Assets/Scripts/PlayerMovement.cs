using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] float speed;
    [SerializeField] int health;
    [Header("Directions/Physics")]
    private Vector2 input;
    private Rigidbody2D rb;
    [Header("UI parameters")]
    [SerializeField] TextMeshProUGUI hpBarText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = speed * input;
    }
    public void takeDamage(int takenDamage)
    {
        health -= takenDamage;
        //the thing where we check if hp is 0,if is:make some other function
        hpBarText.text = health.ToString() + "/100";
    }
}
