using UnityEngine;

public class BulletsScript : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    float despawnTimer;
    void OnEnable()
    {
        despawnTimer = 0f;
    }
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        despawnTimer+= Time.deltaTime;
        if(despawnTimer >= 5f)
        {
            Destroy(this.gameObject);
        }
    }

}
