using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSwitchingScript : MonoBehaviour
{
    [SerializeField]List<GameObject> allWeapons;
    public enum equippedWeapon {gun, sword}
    equippedWeapon currentWeapon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWeapon = equippedWeapon.sword;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EquipANewWeapon(string newWeapon)
    {
        switch (newWeapon)
        {
            case "gun":
                currentWeapon = equippedWeapon.gun;
                foreach(GameObject weapon in allWeapons)
                {
                    weapon.gameObject.SetActive(false);
                }
                allWeapons[0].SetActive(true);
                break;
            case "sword":
                currentWeapon = equippedWeapon.sword;
                foreach(GameObject weapon in allWeapons)
                {
                    weapon.gameObject.SetActive(false);
                }
                allWeapons[1].SetActive(true);
                break;
        }
    }
}
