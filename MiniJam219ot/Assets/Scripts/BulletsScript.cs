using UnityEngine;

public class BulletsScript : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    public int damageLevel;
    float despawnTimer;
    void OnEnable()
    {
        despawnTimer = 0f;
    }
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

}

                    
             
