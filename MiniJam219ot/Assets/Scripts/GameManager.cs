using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeOfRun;//the timer that is 3 min long and gets enemies stronger and + points for a survival time
    public int countOfDays;
    public static bool isInRun;
    public bool isPerfectRun;//if player survived whole 3 minutes - true(+ points multiplier)
    [SerializeField] PlayerMovement player;
    [SerializeField] TextMeshProUGUI timeLeftText,rewardText;
    [SerializeField] GameObject pickingMenu,spawnManager,afterRunMenu,playerObject;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Object.FindAnyObjectByType<PlayerMovement>();
        countOfDays = 0;
        StartOfGame();
    }
    void StartOfGame()
    {
        WeaponChoice();
        spawnManager.SetActive(true);
        timeOfRun = 180f;
        isInRun = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(isInRun)
        {
            timeOfRun -= Time.deltaTime;
            timeLeftText.text = "Survive for:"+(int)timeOfRun+"!";
            if(timeOfRun <= 0)
            {
                EndOfRun();
                isPerfectRun = true;
            }
        }
    }
    public void WeaponChoice()
    {
        pickingMenu.gameObject.SetActive(true);
        SimplePause();
    }
    public void EndOfRun()
    {
        //function of spawn manager to 
        spawnManager.SetActive(false);
        isInRun = false;
        countOfDays += 1;
        if (isPerfectRun)
        {
            rewardText.text = "WOW! you defend the whole wave?! here is your bonus!:" + ((180 - (int)timeOfRun) + (player.points * 1.5));
            player.points += ((180 - (int)timeOfRun) + (player.points * 1.5));
        }
        else
        {
            rewardText.text = "Well done! here is points for survival time:" + (180 - (int)timeOfRun);
            player.points += 180 - (int)timeOfRun;
        }
        countOfDays += 1;
        if (countOfDays == 2)
        {
            EndingShow();
        }
    }
    public void SimplePause()
    {
        Time.timeScale = 0;
    }
    public void EndingShow()
    {
         //player score if statement >250 bad <250 good
    }
    
}
