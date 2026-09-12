using UnityEngine;
using UnityEngine.Events;

public class UpgradeScript : MonoBehaviour
{
    public UnityEvent<string> onUpgrade;
    PlayerMovement playerMovementScript;
    int healthLevel;
    int speedLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        onUpgrade.AddListener(playerMovementScript.UpgradeStat);
        healthLevel = 1;
        speedLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void sendAnUpgrade(string upgradeToSend)
    {
        onUpgrade.Invoke(upgradeToSend);
        switch (upgradeToSend)
        {
            case "speed":
                speedLevel+=1;
                break;
            case "health":
                healthLevel+=1;
                break;
        }
    }
}
