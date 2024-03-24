using System.Collections.Generic;
using UnityEngine;
using Pathing;
using System.Linq;
using System;
using Zenject;

public class Tile : MonoBehaviour, IAStarNode, ITile
{
  //public (int X, int Z) coordinates;
  public (int X, int Z) Coordinates {get;set;}
  public Material DefaultMaterial { get; private set; }
  [field: SerializeField] public int Cost { get; private set; }
  [field: SerializeField] public Material HighlightMaterial { get; private set; }
  [field: SerializeField] public Material SelectedMaterial { get; private set; }
  [field: SerializeField] public Material PartOfThePathMaterial { get; private set; }
  [Inject]
  private TileMap _tileMap;

  [Inject]
    public void Construct(TileMap tileMap)
    {
        _tileMap = tileMap;
        InitializeTile();
    }
  private void InitializeTile()
    {
        DefaultMaterial = GetComponent<Renderer>().material;
    }
    void Awake()
    {
      _tileMap = FindObjectOfType<TileMap>();
    }
  private void AddNeighbour(Tile? tile, List<Tile> neighbours)
  {
    if (tile != null)
    {
      neighbours.Add(tile);
    }
  }
  public IEnumerable<IAStarNode> Neighbours
  {
    get { return GetNeighbours(); }
  }

  public IEnumerable<Tile> GetNeighbours()
  {
    Debug.Log("tilemap value: " + _tileMap);
    List<Tile> neighbourList = new();
    bool oddRow = Coordinates.Z % 2 == 1;
    AddNeighbour(_tileMap.FindTile((Coordinates.X - 1, Coordinates.Z)), neighbourList);//left
    AddNeighbour(_tileMap.FindTile((Coordinates.X + 1, Coordinates.Z)), neighbourList);//right
    if (oddRow)
    {
      AddNeighbour(_tileMap.FindTile((Coordinates.X, Coordinates.Z + 1)), neighbourList);
      AddNeighbour(_tileMap.FindTile((Coordinates.X - 1, Coordinates.Z + 1)), neighbourList);
      AddNeighbour(_tileMap.FindTile((Coordinates.X - 1, Coordinates.Z - 1)), neighbourList);
      AddNeighbour(_tileMap.FindTile((Coordinates.X, Coordinates.Z - 1)), neighbourList);
    }
    else
    {
      AddNeighbour(_tileMap.FindTile((Coordinates.X + 1, Coordinates.Z - 1)), neighbourList);
      AddNeighbour(_tileMap.FindTile((Coordinates.X, Coordinates.Z + 1)), neighbourList);
      AddNeighbour(_tileMap.FindTile((Coordinates.X, Coordinates.Z - 1)), neighbourList);
      AddNeighbour(_tileMap.FindTile((Coordinates.X + 1, Coordinates.Z + 1)), neighbourList);
    }
    return neighbourList.AsEnumerable();
  }

  public float CostTo(IAStarNode neighbour)
  {
    return GetCostToNeighbour((Tile)neighbour);
  }
  private float GetCostToNeighbour(Tile n)
  {
    return n.Cost;
  }

  public float EstimatedCostTo(IAStarNode goal)
  {
    return GetEstimatedCost((Tile)goal);
  }
  private float GetEstimatedCost(Tile goal)
  {
    float distance = Mathf.Abs(Coordinates.X - goal.Coordinates.X) + MathF.Abs(Coordinates.Z - goal.Coordinates.Z);
    return distance * Cost;
  }
}
