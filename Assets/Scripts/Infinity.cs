using UnityEngine;

public class Infinity : MonoBehaviour
{

    public int Count;
    public float scrollspeed;

    void Start()
    {
        Count = transform.childCount;
    }
    void Update()
    {
        float totalspeed = scrollspeed * Time.deltaTime * 1;
        transform.Translate(totalspeed, 0, 0); 
    }
    void LateUpdate() {
         
        if (transform.position.x > 15) {
           transform.Translate(-30,0,0, Space.Self);
        }

    
  
    }
    
}
