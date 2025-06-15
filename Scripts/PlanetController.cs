using TMPro;
using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public float rescaleSpeed = 0.005f;         //Adjustment of rescale speed
    public float rotateSpeed = 10f;             //Adjustment of rotation speed
    private float timeSinceLetGo = 0f;          //Time of the last tap
    private float doubleTapThreshold = 0.5f;    //Max time between taps to be considered a double-tap
    public GameObject infoPanel;                //Object's UI panel
    private CameraRayCast cameraRayCast;        //Reference to camera ray cast script

    private void Start()
    {
        //Find CameraRaycast script attached to main camera
        cameraRayCast = Camera.main.GetComponent<CameraRayCast>();
    }

    void Update()
    {
        //Raycast-based palent selection with double-tap logic
        if (Input.GetMouseButtonUp(0)) timeSinceLetGo = Time.unscaledTime;                                     
        else if (Input.GetMouseButtonDown(0) && (Time.unscaledTime - timeSinceLetGo) <= doubleTapThreshold) 
        {
            timeSinceLetGo = 0;                                     //Reset the double-tap timer
            GameObject hitObject = cameraRayCast.GetRaycastHit();   //Check if raycast hit planet object
            if (hitObject == this.gameObject)                       //If object hit is this planet
                TogglePlanetInfo();                                 //Toggle the planet's info panel
        }

        // Handle touch input for rotation and rescaling
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Handle rotation
            if (touch.phase == TouchPhase.Moved)
            {
                Rotate();
            }
        }
        else if (Input.touchCount == 2)
        {
            Rescale();
        }
    }

    //Rescale based on two-finger pinch
    private void Rescale()
    {
        //Get first and second touches on the screen
        Touch firstTouch = Input.GetTouch(0);
        Touch secondTouch = Input.GetTouch(1);

        //Calculate the positions of each touch in previous frame
        float previousTouchDistance = Vector2.Distance(firstTouch.position - firstTouch.deltaPosition, secondTouch.position - secondTouch.deltaPosition);
        float currentTouchDistance = Vector2.Distance(firstTouch.position, secondTouch.position);

        //Calculate the difference in distance (how far fingers moved)
        float distanceDifference = previousTouchDistance - currentTouchDistance;

        //Adjust object scale based on fingers gesture
        transform.localScale += Vector3.one * distanceDifference * rescaleSpeed;
    }
    
    //Rotate based on single-finger drag
    private void Rotate()
    {
        //Get first touch on screen
        Touch touch = Input.GetTouch(0);

        //Check if the touch has moved (i.e. the user is dragging)
        if (touch.phase == TouchPhase.Moved)
        {
            //Get the main camera's transform for camera-relative movement
            Transform cameraTransform = Camera.main.transform;

            //Get swipe movement on x (horizontal) and y (vertical)
            float horizontalSwipe = touch.deltaPosition.x * rotateSpeed * Time.deltaTime;
            float verticalSwipe = touch.deltaPosition.y * rotateSpeed * Time.deltaTime;

            //Calculate rotation axes relative to the camera's orientation
            Vector3 rightAxis = cameraTransform.right;                                      //X-axis relative to the camera
            Vector3 upAxis = cameraTransform.up;                                            //Y-axis relative to the camera

            //Apply the rotations relative to the camera's right (x-axis) and up (y-axis)
            transform.RotateAround(transform.position, rightAxis, -verticalSwipe);          //Vertical swipe rotates around the camera's X-axis
            transform.RotateAround(transform.position, upAxis, horizontalSwipe);            //Horizontal swipe rotates around the camera's Y-axis
        }
    }

    //Toggle the planet info panel on/off
    private void TogglePlanetInfo()
    {
        bool isActive = infoPanel.activeSelf;
        infoPanel.SetActive(!isActive);
    }
}
