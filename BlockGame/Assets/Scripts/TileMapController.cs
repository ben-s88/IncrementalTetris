using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileMapController : MonoBehaviour
{
    Vector3Int[] currentTiles;
    bool discardPiece;
    Vector3Int[] ITiles = new Vector3Int[] { new(0, 0, 0), new(-1, 0, 0), new(1, 0, 0), new(2, 0, 0) };
    [SerializeField]
    Tilemap TM;
    [SerializeField]
    Grid grid;
    Camera cam;
    [SerializeField]
    Tile[] tiles;

    bool pieceFreezeActive = false;

    float timeSinceMove = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        repositionGrid();
        spawnPieceAt(ITiles, new Vector2Int(2, 15));
        //TM.SetTile(new Vector3Int(0, 0, 0), tiles[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame && currentTiles != null)
        {
            moveLeft();
            discardPiece = tileCollision(currentTiles);
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame && currentTiles != null)
        {
            moveRight();
            discardPiece = tileCollision(currentTiles);
        }

        if (pieceFreezeActive)
        {
            return;
        }

        timeSinceMove += Time.deltaTime;
        if (timeSinceMove >= 1.0f)
        {
            if (discardPiece) // if the tile was discarded spawn a new one
            {
                currentTiles = null;
                spawnPieceAt(ITiles, new Vector2Int(2, 15));
                discardPiece = false;
            }

            TM.SetTiles(currentTiles, getTileList(currentTiles.Length, null)); //set the pre update positions on the tile map blank

            for (int i = 0; i < currentTiles.Length; i++)
            {
                currentTiles[i] += new Vector3Int(0, -1, 0); //transform all tiles downward
                if (currentTiles[i].y  == 0) { discardPiece = true; } // if the bottom of the screen declare the tile to be discarded/placed
            }

            TM.SetTiles(currentTiles, getTileList(currentTiles.Length, tiles[0])); // set the new positions to the selected tile asset

            if (tileCollision(currentTiles))
            {
                discardPiece = true;
            } 

            /*if (discardPiece)
            {
                currentTiles = null; //set tile to null so we know to create a new one
                discardPiece = false;
            }*/

            timeSinceMove = 0.0f;
        }
    }

    void repositionGrid()
    {
        grid.transform.position = cam.ViewportToWorldPoint(new Vector3(0.1f, 0.05f, 10));
    }

    void spawnPieceAt(Vector3Int[] positions, Vector2Int startPos)
    {
        currentTiles = (Vector3Int[])positions.Clone();
        for (int i = 0; i < currentTiles.Length; i++)
        {
            currentTiles[i] += new Vector3Int(startPos.x, startPos.y, 0);
        }
        TM.SetTiles(currentTiles, getTileList(positions.Length, tiles[0]));
        
    }

    public void moveLeft()
    {
        int lowestX = 10000;
        List<Vector3Int> leftmostTiles = new List<Vector3Int>();

        for (int i = 0; i < currentTiles.Length; i++)
        {
            if (currentTiles[i].x < lowestX)
            {
                leftmostTiles = new List<Vector3Int>();
                lowestX = currentTiles[i].x;
            }

            if (currentTiles[i].x == lowestX)
            {
                leftmostTiles.Add(currentTiles[i]);
            }
        }

        foreach (Vector3Int t in leftmostTiles)
        {
            if (t.x == -1) { return; }
            if (TM.GetTile(t + new Vector3Int(-1, 0, 0)) != null) { return; }
        }

        TM.SetTiles(currentTiles, getTileList(currentTiles.Length, null));

        for (int i = 0; i < currentTiles.Length; i++)
        {
            currentTiles[i] += new Vector3Int(-1, 0, 0);
        }

        TM.SetTiles(currentTiles, getTileList(currentTiles.Length, tiles[0]));
    }

    public void moveRight()
    {
        int highestX = -10000;
        List<Vector3Int> rightmostTiles = new List<Vector3Int>();

        for (int i = 0; i < currentTiles.Length; i++)
        {
            if (currentTiles[i].x > highestX)
            {
                rightmostTiles = new List<Vector3Int>();
                highestX = currentTiles[i].x;
            }

            if (currentTiles[i].x == highestX)
            {
                rightmostTiles.Add(currentTiles[i]);
            }
        }

        foreach (Vector3Int t in rightmostTiles)
        {
            if (t.x == 8) { return; }
            if (TM.GetTile(t + new Vector3Int(1, 0, 0)) != null) { return; }
        }

        TM.SetTiles(currentTiles, getTileList(currentTiles.Length, null));

        for (int i = 0; i < currentTiles.Length; i++)
        {
            currentTiles[i] += new Vector3Int(1, 0, 0);
        }

        TM.SetTiles(currentTiles, getTileList(currentTiles.Length, tiles[0]));
    }

    //make a send piece down function

    bool tileCollision(Vector3Int[] currTiles)
    {
        int lowestY = 10000;
        List<Vector3Int> lowestTiles = new List<Vector3Int>();

        for (int i = 0;i < currentTiles.Length; i++)
        {
            if (currTiles[i].y < lowestY)
            {
                lowestTiles = new List<Vector3Int>();
                lowestY = currTiles[i].y;
            }

            if (currTiles[i].y == lowestY)
            {
                lowestTiles.Add(currTiles[i]);
            }
        }

        foreach (Vector3Int t in lowestTiles)
        {
            if (TM.GetTile(t - new Vector3Int(0, 1, 0)) != null) { return true; }
        }

        return false;
    }

    Tile[] getTileList(int length, Tile tile)
    {
        Tile[] tiles_ = new Tile[length];
        for (int i = 0; i < length; i++)
        {
            tiles_[i] = tile;
        }
        return tiles_;
    }

    public void setPieceFreeze()
    {
        if (pieceFreezeActive) {  return; }
        pieceFreezeActive = true;
        Debug.Log("Piece frozen");
        disablePieceFreeze();
    }

    IEnumerable disablePieceFreeze()
    {
        yield return new WaitForSeconds(2);
        pieceFreezeActive=false;
    }
}
