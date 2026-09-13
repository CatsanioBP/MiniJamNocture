using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UpgradeBoxScript : MonoBehaviour
{
    public GameObject player;
    public GameObject gun;
    public GameObject sword;
    string[] elementList;
    string[] generalUpgradeList;
    string[] powerList;
    //FOR SENDING
    string chosenUpgrade;
    public UnityEvent<string> sendUpgrade;
    //FOR SENDING ^^^^^
    int randomUpgradeTypeNumber;
    int randomUpgradeNumber;
    public GameObject upgradeUI;
    
    public Button[] upgradeButtons = new Button[3];
    TextMeshProUGUI[] upgradesText = new TextMeshProUGUI[3];
    UnityEngine.UI.Image[] upgradeImage = new UnityEngine.UI.Image[3];
    [SerializeField]List<Texture2D> imagesForUpgrades; 
    string[] upgradesOnButtons = new string[3];
    int buttonThatSent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sendUpgrade.AddListener(player.GetComponent<PlayerMovement>().UpgradeStat);
        sendUpgrade.AddListener(sword.GetComponentInChildren<SwordScript>().onUpgradeChoose);
        sendUpgrade.AddListener(gun.GetComponentInChildren<Weapon>().onUpgradeChoose);
        elementList = new string[] {"fire", "ice", "wind"};
        generalUpgradeList = new string[] {"speed", "health", "reload", "damage"};
        powerList = new string[] {"dash", "slowdown", "immunity"};
        
        for(int i = 0; i < 3; i++)
        {
            upgradesText[i] = upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            upgradeImage[i] = upgradeButtons[i].GetComponent<UnityEngine.UI.Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            foreach(Button button in upgradeButtons)
            {
            button.GetComponent<boxButtonScript>().clickFuncEvent.AddListener(sendUpgradeFunc);
            }
            Debug.Log("SELECT COLLISION");
            CreateUpgrades();
            upgradeUI.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
    void CreateUpgrades()
    {
        for(int i = 0; i < 3; i++)
        {
            randomUpgradeTypeNumber = Random.Range(1, 101);
            if( randomUpgradeTypeNumber <= 40) //player stat
            {
                randomUpgradeNumber = Random.Range(0, generalUpgradeList.Count());
                chosenUpgrade = generalUpgradeList[randomUpgradeNumber];
                putUpgradeOnBox(i, chosenUpgrade);
                upgradesOnButtons[i] = chosenUpgrade;
            }
            else if(randomUpgradeTypeNumber > 40 && randomUpgradeTypeNumber <= 75)// elemental
            {
                randomUpgradeNumber = Random.Range(0, elementList.Count());
                chosenUpgrade = elementList[randomUpgradeNumber];
                putUpgradeOnBox(i, chosenUpgrade);
                upgradesOnButtons[i] = chosenUpgrade;
            }
            else //ability
            {
                randomUpgradeNumber = Random.Range(0, powerList.Count());
                chosenUpgrade = powerList[randomUpgradeNumber];
                putUpgradeOnBox(i, chosenUpgrade);
                upgradesOnButtons[i] = chosenUpgrade;
            }

        }
    }
    void putUpgradeOnBox(int i, string upgradeText) //UI Box
    {
        upgradesText[i].text = upgradeText;
        upgradeImage[i].sprite = Sprite.Create(imagesForUpgrades.Find(p => p.name == upgradeText), new Rect(0, 0, imagesForUpgrades.Find(p => p.name == upgradeText).width, imagesForUpgrades.Find(p => p.name == upgradeText).height), new Vector2(0.5f, 0.5f));
    }
    public void sendUpgradeFunc(int i)
    {
        sendUpgrade.Invoke(upgradesOnButtons[i]);
        Debug.Log("BUTTON THAT SENT " + i);
        Destroy(gameObject);
    }
    
    
    // void OnDisable()
    // {
    //     Time.timeScale = 1;
    //     upgradeUI.gameObject.SetActive(false);
    // }
}
