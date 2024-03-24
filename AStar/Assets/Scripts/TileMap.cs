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
  private List<(int, int)> deserts;
  private List<(int, int)> forests;
  private List<(int, int)> mountains;
  private List<(int, int)> waters;
  [SerializeField] private List<GameObject> tilePrefabs = new(5);
  // Start is called before the first frame update
  void Start()
  {
    GenerateMapBlueprint();
    SpawnTiles();
  }

  private void GenerateMapBlueprint()
  {
    tilesCollection = new List<Tile>();
    //populate map data according to the image in a task document
    deserts = new List<(int, int)> { (2, 2), (7, 3), (6, 4), (0, 4), (7, 4), (0, 5), (2, 5), (6, 5), (0, 6), (1, 6), (2, 6), (0, 7), (1, 7) };
    forests = new List<(int, int)> { (3, 0), (5, 0), (1, 1), (3, 1), (4, 1), (0, 2), (1, 2), (3, 3), (5, 3), (1, 4), (3, 4), (5, 4), (1, 5), (3, 6), (5, 6), (7, 6), };
    mountains = new List<(int, int)> { (0, 0), (1, 0), (2, 0), (2, 1), (4, 3), (4, 4), (5, 5), (6, 6), (5, 7), (6, 7) };
    waters = new List<(int, int)> { (7, 0), (6, 1), (7, 1), (3, 7), (4, 2), (5, 2), (7, 2), (0, 3), (1, 3), (2, 3), (2, 4) };
    for (int x = 0; x < MapSizeX; x += 1)
    {
      for (int z = 0; z < MapSizeZ; z += 1)
      {
        if (deserts.Contains((x, z)))
        {
          mapData.Add((x, z), 1);
        }
        if (mountains.Contains((x, z)))
        {

          mapData.Add((x, z), 2);
        }
        if (forests.Contains((x, z)))
        {

          mapData.Add((x, z), 3);
        }
        if (waters.Contains((x, z)))
        {

          mapData.Add((x, z), 4);
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
        tile.GetComponentInChildren<TextMeshProUGUI>().text = tile.Coordinates.X + "," + tile.Coordinates.Z;
      }

    }
  }
  
  public Tile FindTile((int x, int z) coords)
  {
    foreach (var t in tilesCollection)
    {
      if(t.Coordinates == coords && t.Cost != -1)
      return t;
    }
    return null;
  }
}
