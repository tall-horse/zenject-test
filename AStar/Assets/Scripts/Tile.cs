using System.Collections.Generic;
using UnityEngine;
using Pathing;
using System.Linq;
using System;

public class Tile : MonoBehaviour, IAStarNode
{
  public (int X, int Z) coordinates;
  public Material DefaultMaterial { get; private set; }
  [field: SerializeField] public int Cost { get; private set; }
  [field: SerializeField] public Material HighlightMaterial { get; private set; }
  [field: SerializeField] public Material SelectedMaterial { get; private set; }
  [field: SerializeField] public Material PartOfThePathMaterial { get; private set; }
  private TileMap tileMap;

  void Start()
  {
    tileMap = FindObjectOfType<TileMap>();
    DefaultMaterial = GetComponent<Renderer>().material;
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
    List<Tile> neighbourList = new();
    bool oddRow = coordinates.Z % 2 == 1;

    AddNeighbour(tileMap.FindFile((coordinates.X - 1, coordinates.Z)), neighbourList);//left
    AddNeighbour(tileMap.FindFile((coordinates.X + 1, coordinates.Z)), neighbourList);//right
    if (oddRow)
    {
      AddNeighbour(tileMap.FindFile((coordinates.X, coordinates.Z + 1)), neighbourList);
      AddNeighbour(tileMap.FindFile((coordinates.X - 1, coordinates.Z + 1)), neighbourList);
      AddNeighbour(tileMap.FindFile((coordinates.X - 1, coordinates.Z - 1)), neighbourList);
      AddNeighbour(tileMap.FindFile((coordinates.X, coordinates.Z - 1)), neighbourList);
    }
    else
    {
      AddNeighbour(tileMap.FindFile((coordinates.X + 1, coordinates.Z - 1)), neighbourList);
      AddNeighbour(tileMap.FindFile((coordinates.X, coordinates.Z + 1)), neighbourList);
      AddNeighbour(tileMap.FindFile((coordinates.X, coordinates.Z - 1)), neighbourList);
      AddNeighbour(tileMap.FindFile((coordinates.X + 1, coordinates.Z + 1)), neighbourList);
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
    float distance = Mathf.Abs(coordinates.X - goal.coordinates.X) + MathF.Abs(coordinates.Z - goal.coordinates.Z);
    return distance * Cost;
  }
}
