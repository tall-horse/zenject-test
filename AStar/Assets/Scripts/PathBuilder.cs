using System.Collections.Generic;
using System.Linq;
using Pathing;
using UnityEngine;

public class PathBuilder : MonoBehaviour
{
  private const float StartEndTileLiftValue = 0.2f;
  private const string SelectableTag = "Selectable";
  private Material defaultMaterial;
  private Transform _selection;
  private Ray ray;
  private RaycastHit hit;
  private Camera playerCamera;
  private TileMap tileMap;
  private List<Tile> path;
  private Tile startTile = null;
  private Tile finishTile = null;
  // Start is called before the first frame update
  void Start()
  {
    playerCamera = Camera.main;
    tileMap = FindObjectOfType<TileMap>();
  }

  // Update is called once per frame
  void Update()
  {
    HighlightTile();
    PickTile();
  }

  private void PickTile()
  {
    if (_selection == null)
      return;
    if (Input.GetMouseButtonUp(0))
    {
      PickStartTile();
    }
    if (Input.GetMouseButtonUp(1))
    {
      PickFinishTile();
    }
  }

  private void ResetSelections()
  {
    foreach (var tile in tileMap.tilesCollection)
    {
      tile.transform.position = new Vector3(tile.transform.position.x, 0, tile.transform.position.z);
      tile.GetComponent<Renderer>().material = tile.DefaultMaterial;
    }
  }

  private void PickStartTile()
  {
    if(startTile != null && finishTile != null)
    {
      startTile = null;
    }
    startTile = _selection.GetComponent<Tile>();
    startTile.transform.position = new Vector3(startTile.transform.position.x, startTile.transform.position.y + StartEndTileLiftValue, startTile.transform.position.z);
    startTile.GetComponent<Renderer>().material = startTile.SelectedMaterial;
    if (startTile != null && finishTile != null)
    {
      BuildPath();
    }
  }

  private void PickFinishTile()
  {
    if(startTile == null)
      return;
    finishTile = _selection.GetComponent<Tile>();
    finishTile.transform.position = new Vector3(finishTile.transform.position.x, finishTile.transform.position.y + StartEndTileLiftValue, finishTile.transform.position.z);
    finishTile.GetComponent<Renderer>().material = finishTile.SelectedMaterial;
    if (startTile != null && finishTile != null)
    {
      BuildPath();
    }
  }

  private void BuildPath()
  {
    if (path != null)
    {
      ResetSelections();
    }
    path = new();
    foreach (var tile in AStar.GetPath(startTile, finishTile).ToList())
    {
      path.Add((Tile)tile);
    }
    ShowPath();
  }

  private void ShowPath()
  {
    foreach (var tile in path)
    {
      if (tile != startTile && tile != finishTile)
      {
        tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + StartEndTileLiftValue, tile.transform.position.z);
        tile.GetComponent<Renderer>().material = tile.PartOfThePathMaterial;
      }
      else
      {
        if (tile.transform.position.y == 0)
        {
          tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + StartEndTileLiftValue, tile.transform.position.z);
        }
        tile.GetComponent<Renderer>().material = tile.HighlightMaterial;
      }
    }
  }

  private void HighlightTile()
  {
    if (_selection != null)
    {
      var selectionRenderer = _selection.GetComponent<Renderer>();
      if (_selection.GetComponent<Tile>() != startTile && _selection.GetComponent<Tile>() != finishTile)
        selectionRenderer.material = defaultMaterial;
      _selection = null;
    }
    ray = playerCamera.ScreenPointToRay(Input.mousePosition);

    if (Physics.Raycast(ray, out hit))
    {
      var selection = hit.transform;
      if (selection.CompareTag(SelectableTag))
      {
        var selectionRenderer = selection.GetComponent<Renderer>();
        defaultMaterial = selectionRenderer.material;
        if (selectionRenderer != null)
        {
          selectionRenderer.material = selectionRenderer.GetComponent<Tile>().HighlightMaterial;
        }
        _selection = selection;
      }
    }
  }
}
