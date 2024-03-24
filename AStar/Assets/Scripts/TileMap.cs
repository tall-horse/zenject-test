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
  private Dictionary<(int, int), int> biomeData = new Dictionary<(int, int), int>();
  // Start is called before the first frame update
  void Start()
  {
    GenerateMapBlueprint();
    SpawnTiles();
  }
  private void GenerateMapBlueprint()
  {
    tilesCollection = new List<Tile>();

    List<KeyValuePair<(int, int), int>> itemsToAdd = new()
    {
    new((2, 2), 1),
    new((7, 3), 1),
    new((6, 4), 1),
    new((0, 4), 1),
    new((7, 4), 1),
    new((0, 5), 1),
    new((2, 5), 1),
    new((6, 5), 1),
    new((0, 6), 1),
    new((1, 6), 1),
    new((2, 6), 1),
    new((0, 7), 1),
    new((1, 7), 1),

    new((3, 0), 0),
    new((5, 0), 0),
    new((1, 1), 0),
    new((3, 1), 0),
    new((4, 1), 0),
    new((0, 2), 0),
    new((1, 2), 0),
    new((3, 3), 0),
    new((5, 3), 0),
    new((1, 4), 0),
    new((3, 4), 0),
    new((5, 4), 0),
    new((1, 5), 0),
    new((3, 6), 0),
    new((5, 6), 0),
    new((7, 6), 0),

    new((0, 0), 2),
    new((1, 0), 2),
    new((2, 0), 2),
    new((2, 1), 2),
    new((4, 3), 2),
    new((4, 4), 2),
    new((5, 5), 2),
    new((6, 6), 2),
    new((5, 7), 2),
    new((6, 7), 2),

    new((7, 0), 4),
    new((6, 1), 4),
    new((7, 1), 4),
    new((3, 7), 4),
    new((4, 2), 4),
    new((5, 2), 4),
    new((7, 2), 4),
    new((0, 3), 4),
    new((1, 3), 4),
    new((2, 3), 4),
    new((2, 4), 4)
};
foreach (var item in itemsToAdd)
{
  biomeData.Add(item.Key, item.Value);
}

    //populate map data according to the image in a task document
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
        tile.GetComponentInChildren<TextMeshProUGUI>().text = tile.Coordinates.X + "," + tile.Coordinates.Z;
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
