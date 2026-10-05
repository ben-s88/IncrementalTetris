using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    GameObject TileMap;
    TileMapController tmc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmc = TileMap.GetComponent<TileMapController>();

        if (!SystemInfo.supportsAccelerometer)
        {
            Debug.Log("Device does not support gyro");
        }
        InputSystem.EnableDevice(Accelerometer.current);
        Accelerometer.current.samplingFrequency = 60f;
    }

    // Update is called once per frame
    void Update()
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
