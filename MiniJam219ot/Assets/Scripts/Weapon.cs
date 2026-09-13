using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum element{fire, wind, ice, none}
    public float knockbackStrength;
    public int elementalLevel;
    public element currentElement;
    [SerializeField] SpriteRenderer weaponSprite;
    [SerializeField] GameObject bulletPrefab;
    public Transform firePoint;
    int reloadLevel;
    int damageLevel;

    public float reloadTime;
    private float currentReloadTime = 0f;
    //private PauseScript pause;
    private Camera mainCamera;
    private Transform cachedTransform;

    private void Start()
    {
        mainCamera = Camera.main;
        cachedTransform = transform;
        //pause = Object.FindAnyObjectByType<PauseScript>();
    }
    void Update()
    {
        if (PauseScript.isPaused) return;
        if (currentReloadTime > 0)
        {
            currentReloadTime -= Time.deltaTime;
        }

        AimMouse();

        if (Input.GetMouseButtonDown(0) && currentReloadTime <= 0f)
        {
            Shoot();
        }
        //if (!pause.isPaused)
        //{
        //    if (currentReloadTime > 0)
        //    {
        //        currentReloadTime -= Time.deltaTime;
        //    }
        //
        //    AimMouse();
        //    if (Input.GetMouseButtonDown(0) && currentReloadTime <= 0f)
        //    {
        //        Destroy(Instantiate(bullet, firePoint.transform.position, transform.rotation), 4f);
        //        currentReloadTime += reloadTime;
        //    }
        //}
    }

    void AimMouse()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - cachedTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        cachedTransform.rotation = Quaternion.Euler(0, 0, angle);
        weaponSprite.flipY = (angle > 90 || angle < -90);
        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = mousePosition - transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //
        //transform.rotation = Quaternion.Euler(0, 0, angle);
        //
        //if (angle > 90 || angle < -90)
        //{
        //    weaponSprite.flipY = true;
        //}
        //else
        //{
        //    weaponSprite.flipY = false;
        //}
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, cachedTransform.rotation);
        Destroy(bullet, 3f);
        currentReloadTime = reloadTime; 
        bullet.GetComponent<BulletsScript>().damageLevel = this.damageLevel;
    }
    public void onUpgradeChoose(string chosenUpgrade)
    {
        
        switch (chosenUpgrade)
        {
            case "fire":
                if(currentElement == element.fire)
                {
                    elementalLevel+=1;
                }
                else{
                    currentElement = element.fire;
                    elementalLevel-=1;
                }

                break;
            case "wind":
                if(currentElement == element.wind)
                {
                    elementalLevel+=1;
                }
                else{
                    currentElement = element.wind;
                    elementalLevel-=1;
                }

                break;
            case "ice":
                if(currentElement == element.ice)
                {
                    elementalLevel+=1;
                }
                else{
                    currentElement = element.ice;
                    elementalLevel-=1;
                }
                break;
            case "reload":
                reloadLevel+=1;
                break;
            case "damage":
                damageLevel += 1;
                break;
            
        }

    }
    
}
