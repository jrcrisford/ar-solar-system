using UnityEngine;

public class CameraRayCast : MonoBehaviour
{
    Camera cam;     //Camera component

    void Start()
    {
        cam = GetComponent<Camera>();    
    }

    public GameObject GetRaycastHit()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);        //Ray from camera to hit point
        RaycastHit hit;                                             //Hit point

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);   //DEBUG Drawn ray line

        if (Physics.Raycast(ray, out hit))                          //Perform raycast
            return hit.collider.gameObject;                         //Return the GameObject that was hit

        return null;                                                //Return on object hit
    }
}
