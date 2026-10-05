using System;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UIElements;

public class GridInit : MonoBehaviour
{
    bool[,] grid = new bool[10, 6];
    [SerializeField]
    GameObject quadPrefab;
    GameObject currentShape;
    float timeSinceMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int y = 0; y < 10; y++)
        {
            for(int x = 0; x < 6; x++)
            {
                grid[y, x] = false;
            }
        }

        currentShape = Instantiate(quadPrefab, new Vector3(-2.5f, 4.5f, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentShape) { return; }
        timeSinceMove += Time.deltaTime;
        if (timeSinceMove >= 1)
        {
            currentShape.transform.Translate(Vector3.down);
            timeSinceMove = 0.0f;

            if (currentShape.transform.position.y == -4.5f)
            {

                currentShape = null;
            }
        }
    }
}
