using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    public Transform sun;
    public float orbitSpeed = 10f;
    public bool isPaused = false;

    void Update()
    {
        if (!isPaused)
            transform.RotateAround(sun.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }

    public void ToggleOrbit(bool pauseStatus)
    {
        isPaused = pauseStatus;
    }
}
