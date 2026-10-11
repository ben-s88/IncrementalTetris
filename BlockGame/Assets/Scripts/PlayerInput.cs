using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.TouchPhase;

public class PlayerInput : MonoBehaviour
{
    Vector2 lastTouchFrame;
    int fingerMovement;
    public int moveFactor = 10; //set what percentage of the screen the touch must move to register a piece move
    int factoredW, factoredH; 
    [SerializeField]
    GameObject TileMap;
    TileMapController tmc;
    bool gyro = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmc = TileMap.GetComponent<TileMapController>();
        if (SystemInfo.supportsAccelerometer)
        {
            gyro = true;
            InputSystem.EnableDevice(Accelerometer.current);
            Accelerometer.current.samplingFrequency = 60f;
        }
        else
        {
            Debug.Log("Device does not support gyro");
            gyro = false;
        }

        factoredH = Screen.height / moveFactor; // calculate the pixel values for moves
        factoredW = Screen.width / moveFactor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastTouchFrame = touch.position;
                    break;

                case TouchPhase.Moved:
                    if (math.abs(touch.position.y - lastTouchFrame.y) > factoredW)
                    {

                    }
                    break;
            }
        }

        if (gyro)
        {
            Accelerometer acc = Accelerometer.current;
            Vector3 value = acc.acceleration.ReadValue();
            if (value.magnitude > 0.5f)
            {
                Debug.Log(value.magnitude);
                tmc.setPieceFreeze();
            }
        }
        
    }
}
