using Solo.MOST_IN_ONE;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.TouchPhase;

public class PlayerInput : MonoBehaviour
{
    Vector2 touchStart;
    int fingerMovement;
    public int moveFactor = 8; //set what percentage of the screen the touch must move to register a piece move
    int factoredW, factoredH; 
    [SerializeField]
    GameObject TileMap;
    TileMapController tmc;
    bool gyro = true;
    bool vibration = false;

    public MOST_HapticFeedback.CustomHapticPattern cmp;
    void Start()
    {
        tmc = TileMap.GetComponent<TileMapController>();
        tmc.PI = this;
        if (SystemInfo.supportsVibration)
        {
            vibration = true;
        }
        else
        {
            Debug.Log("Device does not support vibrations");
        }
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
                    touchStart = touch.position;
                    break;

                case TouchPhase.Moved:
                    if (touch.position.x - touchStart.x > factoredW)
                    {
                        tmc.moveRight();
                        touchStart = touch.position;
                    }
                    else if (touch.position.x - touchStart.x < -factoredW)
                    {
                        tmc.moveLeft();
                        touchStart = touch.position;
                    }
                    break;
            }
        }

        if (gyro)
        {
            Accelerometer acc = Accelerometer.current;
            Vector3 value = acc.acceleration.ReadValue();
            if (value.magnitude > 3.0f)
            {
                Debug.Log(value.magnitude);
                tmc.setPieceFreeze();
            }
        }
        
    }

    public void doVibration(int strength)
    {
        if (vibration)
        {
            cmp.Android_HapticPattern[0].PulseStrength = 50 + strength * 30;
            MOST_HapticFeedback.GeneratePattern(cmp);
        }
    }
}
