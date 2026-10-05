using System.Collections;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileMapController : MonoBehaviour
{
    Vector3Int[] currentTiles;
    Vector2Int currentPiecePos;
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
        if (pieceFreezeActive)
        {
            return;
        }

        timeSinceMove += Time.deltaTime;
        if (timeSinceMove >= 1.0f)
        {
            TM.SetTiles(currentTiles, getTileList(currentTiles.Length, null));

            for (int i = 0; i < currentTiles.Length; i++)
            {
                currentTiles[i] += new Vector3Int(0, -1, 0);
            }

            TM.SetTiles(currentTiles, getTileList(currentTiles.Length, tiles[0]));

            timeSinceMove = 0.0f;
        }
    }

    void repositionGrid()
    {
        grid.transform.position = cam.ViewportToWorldPoint(new Vector3(0.1f, 0.05f, 10));
    }

    void spawnPieceAt(Vector3Int[] positions, Vector2Int startPos)
    {
        currentTiles = positions;
        currentPiecePos = startPos;
        for (int i = 0; i < currentTiles.Length; i++)
        {
            currentTiles[i] += new Vector3Int(startPos.x, startPos.y, 0);
        }
        TM.SetTiles(currentTiles, getTileList(positions.Length, tiles[0]));
        
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
