using UnityEngine;

public class Square : MonoBehaviour
{
    [HideInInspector] public GameObject placedObject;

    public bool IsEmpty => placedObject == null;

    public void PlaceObject(GameObject obj)
    {
        placedObject = obj;
    }

    public void RemoveObject()
    {
        placedObject = null;
    }
}
