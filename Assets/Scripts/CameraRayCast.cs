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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = cam.ScreenPointToRay(touch.position);             //Ray from camera to hit point
            RaycastHit hit;                                             //Hit point

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);   //DEBUG Drawn ray line

            if (Physics.Raycast(ray, out hit))                          //Perform raycast
                return hit.collider.gameObject;                         //Return the GameObject that was hit

            return null;                                                //Return if no object was hit
        }
      
        return null;                                                    //Return on object hit
    }
}
