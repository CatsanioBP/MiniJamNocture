using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]GameObject staffGun;
    [SerializeField]GameObject sword;
    [Header("Player Stats")]
    [SerializeField] float speed;
    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] float powerReloadT,maxPowerReloadT;
    public int points;
    public int money;
    [Header("Directions/Physics")]
    private Vector2 input;
    private Rigidbody2D rb;
    [Header("UI parameters")]
    [SerializeField] TextMeshProUGUI hpBarText;
    [SerializeField] TextMeshProUGUI poitsCountText;
    [SerializeField] TextMeshProUGUI moneyCountText;
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
        powerReloadT -= Time.deltaTime;
        if(Input.GetMouseButtonDown(1) && powerReloadT <= 0f)
        {
            StartCoroutine(dashBoost());
        }
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
    public void rewardForKill(int moneyGain,int pointsGain)
    {
        money += moneyGain;
        points += pointsGain;
        poitsCountText.text = "Points:" + points;
        moneyCountText.text = money + "$";

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
                maxHealth+= 25;
                health+=25;
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
    IEnumerator dashBoost()
    {
        speed += 10;
        yield return new WaitForSeconds(1f);
        speed -= 10;
        powerReloadT = maxPowerReloadT;
    }
}
