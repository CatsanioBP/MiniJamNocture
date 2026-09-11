using UnityEngine;

public class BulletsScript : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
