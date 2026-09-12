using Unity.VisualScripting;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    //LEVEL 10 MAX or MAYBE LESS
    [SerializeField] SpriteRenderer weaponSprite;
    public enum element{fire, wind, ice, none}
    BoxCollider2D boxCollider;
    float knockbackStrength;
    int elementalLevel;
    element currentElement;
    public float reloadTime;
    private float currentReloadTime = 0f;
    public float swingTime;
    private float currentSwingTime = 0f;
    private PauseScript pause;
    private Camera mainCamera;
    private Transform cachedTransform;

    void OnEnable()
    {
        weaponSprite.gameObject.SetActive(false);
    }
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        mainCamera = Camera.main;
        cachedTransform = transform;
        elementalLevel = 0;
        currentElement = element.none;
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
            SwingSword();
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
    async void SwingSword()
    {
        weaponSprite.gameObject.SetActive(true);
        boxCollider.enabled = true;
        
  
        await Awaitable.WaitForSecondsAsync(swingTime);
        
        boxCollider.enabled = false;
        weaponSprite.gameObject.SetActive(false);
        
    }
}
