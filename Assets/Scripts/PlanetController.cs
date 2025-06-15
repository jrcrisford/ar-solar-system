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
    private bool isSelected = false;
    private Vector3 initalScale;
    private Quaternion initalRotation;

    private void Start()
    {
        //Find CameraRaycast script attached to main camera
        cameraRayCast = Camera.main.GetComponent<CameraRayCast>();

        initalScale = transform.localScale;
        initalRotation = transform.rotation;
    }

    void Update()
    { 
        if (Input.touchCount > 0)                                           // Check for touch input
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)                            // Single Tap to select and enable rotation/scaling
            {
                GameObject hitObject = cameraRayCast.GetRaycastHit();
                if (hitObject == this.gameObject)
                {
                    isSelected = true; 
                    if (Time.time - timeSinceLetGo < doubleTapThreshold)    //Check for double tap using the custom threshold
                        TogglePlanetInfo();                                 //Toggle the information UI panel if it's a valid double tap
                    timeSinceLetGo = Time.time;                             //Reset timeSinceLetGo for future taps
                }
                else isSelected = false;                                    //Deselect if tapping elsewhere
            }

            if (isSelected)                                                 //Handle Rotation and Scaling when selected
            {
                if (Input.touchCount == 1 && touch.phase == TouchPhase.Moved) Rotate();
                else if (Input.touchCount == 2) Rescale();
            }
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
        float distanceDifference =  currentTouchDistance - previousTouchDistance;

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

    public void ResetTransform()
    {
        transform.localScale = initalScale;
        transform.rotation = initalRotation;
    }
}
