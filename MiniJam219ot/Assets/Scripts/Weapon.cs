using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] SpriteRenderer weaponSprite;
    [SerializeField] GameObject bullet;
    public Transform firePoint;

    void Update()
    {
        AimMouse();
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(Instantiate(bullet,firePoint.transform.position,transform.rotation),4f);
        }
    }

    void AimMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (angle > 90 || angle < -90)
        {
            weaponSprite.flipY = true;
        }
        else
        {
            weaponSprite.flipY = false;
        }
    }
}
