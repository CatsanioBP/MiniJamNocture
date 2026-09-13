using UnityEngine;
using UnityEngine.Events;


public class boxButtonScript : MonoBehaviour
{
    public UnityEvent<int> clickFuncEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //I HATE THIS SCRIPT ITS SO USELESS AND TEMPORARY BUT ITS NEEDED
    public void ClickFunc()
    {
        switch (gameObject.name)
        {
            case "upgrade1":
                clickFuncEvent.Invoke(0);
                break;
            case "upgrade2":
                clickFuncEvent.Invoke(1);
                break;
            case "upgrade3":
                clickFuncEvent.Invoke(2);
                break;
        }
    }
}
