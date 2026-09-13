using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;
using Unity.UI;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject staffGun;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject iceZone;
    [SerializeField] SpriteRenderer playerSprite;
    [Header("Player Stats")]
    [SerializeField] float speed;
    public float speedBoost;//value that adds up to speed when you use dash power
    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] float powerReloadT, maxPowerReloadT;
    public double points;
    public int money;
    [SerializeField] bool isImmune;
    [Header("Directions/Physics")]
    private Vector2 input;
    private Rigidbody2D rb;
    [Header("UI parameters")]
    [SerializeField] Image rechargeFill;
    //[SerializeField] TextMeshProUGUI rechargeText;
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
        if (powerReloadT >= 0)
        {
            powerReloadT -= Time.deltaTime;
            rechargeFill.fillAmount = powerReloadT/10;
            //rechargeText.text = (int)powerReloadT + "";
           
        }
        
        if (Input.GetMouseButtonDown(1) && powerReloadT <= 0f)
        {
            StartCoroutine(iceZonePower());
            //StartCoroutine(imunnityPower());
            //StartCoroutine(dashBoost());
        }
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = speed * input;
    }
    public void takeDamage(int takenDamage)
    {
        if (!isImmune)
        {
            health -= takenDamage;
            //the thing where we check if hp is 0,if is:make some other function
            updateHealth();
        }
    }
    
    public void rewardForKill(int moneyGain, int pointsGain)
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
                speed += 0.5f;
                break;
            case "health":
                maxHealth += 20;
                health += 20;
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
    IEnumerator dashBoostPower()
    {
        speed += speedBoost;
        yield return new WaitForSeconds(1f);
        speed -= speedBoost;
        powerReloadT = maxPowerReloadT;
    }
    IEnumerator imunnityPower()
    {
        isImmune = true;
        playerSprite.color = Color.grey;
        yield return new WaitForSeconds(3f);
        playerSprite.color = Color.white;
        isImmune = false;
        powerReloadT = maxPowerReloadT;
    }
    IEnumerator iceZonePower()
    {
        iceZone.SetActive(true);
        yield return new WaitForSeconds(3f);
        iceZone.SetActive(false);
        powerReloadT = maxPowerReloadT;
    }
}
