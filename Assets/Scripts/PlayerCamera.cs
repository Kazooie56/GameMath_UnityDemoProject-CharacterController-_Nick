using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;

    // Mouse settings
    public float sensitivity = 200f;

    private float minPitch = -89f; //Pitch is up and down
    private float maxPitch = 89f; // 89f and -89f are both 1 degree off from 90 where we'd look straight up or straight downwards
    private float pitch = 0f; // 0f represents where exactly our mouse look direction is, specifically on the y axis


    void Start()
    {

    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; //without Time.deltaTime, your mouse movements are super sensitive

        // rotate player horizontally
        transform.parent.Rotate(Vector3.up, mouseX);                // on the y, rotate left and right, aka yaw

        // rotate camera vertically
        pitch -= mouseY;                                            // pitch is our mouseY movement
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);             // prevent our pitch from going lower than our min, or higher than our max
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);  // rotates the x, y and z
    }
}
