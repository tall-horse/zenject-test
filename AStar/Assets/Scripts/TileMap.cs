using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Zenject;

public class TileMap : MonoBehaviour
{
  public const int MapSizeX = 8;
  public const int MapSizeZ = 8;
  public Dictionary<(int X, int Z), int> mapData = new();
  [HideInInspector] public List<Tile> tilesCollection;
  private const float OffsetX = -0.5f;
  private const float OffsetZ = -0.26f;
  [SerializeField] private List<GameObject> tilePrefabs = new(5);
  private Dictionary<(int, int), int> biomeData;
  // Start is called before the first frame update
  void Start()
  {
    GenerateMapBlueprint();
    SpawnTiles();
  }
  private void GenerateMapBlueprint()
{
    tilesCollection = new List<Tile>();

    biomeData = new Dictionary<(int, int), int>
    {
        {(2, 2), 1},
        {(7, 3), 1},
        {(6, 4), 1},
        {(0, 4), 1},
        {(7, 4), 1},
        {(0, 5), 1},
        {(2, 5), 1},
        {(6, 5), 1},
        {(0, 6), 1},
        {(1, 6), 1},
        {(2, 6), 1},
        {(0, 7), 1},
        {(1, 7), 1},

        {(3, 0), 0},
        {(5, 0), 0},
        {(1, 1), 0},
        {(3, 1), 0},
        {(4, 1), 0},
        {(0, 2), 0},
        {(1, 2), 0},
        {(3, 3), 0},
        {(5, 3), 0},
        {(1, 4), 0},
        {(3, 4), 0},
        {(5, 4), 0},
        {(1, 5), 0},
        {(3, 6), 0},
        {(5, 6), 0},
        {(7, 6), 0},

        {(0, 0), 2},
        {(1, 0), 2},
        {(2, 0), 2},
        {(2, 1), 2},
        {(4, 3), 2},
        {(4, 4), 2},
        {(5, 5), 2},
        {(6, 6), 2},
        {(5, 7), 2},
        {(6, 7), 2},

        {(7, 0), 4},
        {(6, 1), 4},
        {(7, 1), 4},
        {(3, 7), 4},
        {(4, 2), 4},
        {(5, 2), 4},
        {(7, 2), 4},
        {(0, 3), 4},
        {(1, 3), 4},
        {(2, 3), 4},
        {(2, 4), 4}
    };

    //populate map data according to the biomeData
    for (int x = 0; x < MapSizeX; x++)
    {
        for (int z = 0; z < MapSizeZ; z++)
        {
            if (biomeData.ContainsKey((x, z)))
            {
                mapData.Add((x, z), biomeData[(x, z)]);
            }
            else
            {
                mapData.Add((x, z), 0); // Default to empty tile
            }
        }
    }
}
  private void SpawnTiles()
  {
    for (int x = 0; x < MapSizeX; x++)
    {
      for (int z = 0; z < MapSizeZ; z++)
      {
        Tile tile;
        Vector3 spawnPosition;
        //Giving offset for spawned tiles 
        if (z % 2 > 0)
        {
          spawnPosition = new Vector3(x + OffsetX, 0, z + OffsetZ * z);
        }
        else
        {
          spawnPosition = new Vector3(x, 0, z + OffsetZ * z);
        }
        tile = Instantiate(tilePrefabs[mapData.FirstOrDefault(t => t.Key == (x, z)).Value].gameObject, spawnPosition, Quaternion.identity).GetComponent<Tile>();
        tile.Construct(this);
        tile.transform.SetParent(transform);
        tile.Coordinates = (x, z);
        tilesCollection.Add(tile);
        tile.TextRespresentation.text = tile.Coordinates.X + "," + tile.Coordinates.Z;
      }

    }
  }

  public Tile FindTile((int x, int z) coords)
  {
    foreach (var t in tilesCollection)
    {
      if (t.Coordinates == coords && t.Cost != -1)
        return t;
    }
    return null;
  }
}
