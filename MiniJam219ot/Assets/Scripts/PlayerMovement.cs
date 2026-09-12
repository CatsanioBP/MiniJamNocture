using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]GameObject staffGun;
    [SerializeField]GameObject sword;
    [Header("Player Stats")]
    [SerializeField] float speed;
    [SerializeField] int health;
    [SerializeField] int maxHealth;
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
        updateHealth();
    }
    void updateHealth()
    {
        hpBarText.text = health.ToString() + "/" + maxHealth.ToString();
    }
    public void UpgradeStat(string stat)
    {
        switch (stat)
        {
            case "speed":
                speed+=0.5f;
                break;
            case "health":
                maxHealth+= 20;
                health+=20;
                updateHealth();
                break;
        }
    }
    public void chooseWeaponAtStart(string chosenWeapon)
    {
        switch (chosenWeapon)
        {
            case "staff":
                staffGun.gameObject.SetActive(true);
                sword.gameObject.SetActive(false);
                break;
            case "sword":
                sword.gameObject.SetActive(true);
                staffGun.gameObject.SetActive(false);
                break;
        }
    }
    
}
