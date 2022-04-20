using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class Board : MonoBehaviour
{
    public static event Action<HexTile<Tile>> SelectTileEvent;

    public HexMouse hexMouse;
    public HexMap<Tile, int> hexMap;

    public HexTile<Tile> tileHover;
    public HexTile<Tile> tileSelection;

    [Header("UI")] public Canvas canvas;

    public GameObject forewarningTab;
    private Forewarning forewarningController;

    [Header("Audio")] public AudioClipSO tileHoverSfx;
    public AudioClipSO tilePlaceSfx;
    public AudioClipSO tileSelectionSfx;
    public AudioClipSO unitPlacementSfx;

    public GameObject edgePrefab;
    public bool animateTileSpawn;

    [Header("ParticleEffects")] public GameObject spawnUnitEffect;
    public GameObject spawnEnemyEffect;

    private GameObject[] edges;
    private GameObject[] tiles;
    private List<Unit> highlightedUnits = new List<Unit>();

    private void Awake()
    {
        hexMouse = gameObject.AddComponent<HexMouse>();

        forewarningController = forewarningTab.GetComponent<Forewarning>();
        forewarningTab.SetActive(false);
    }

    void Update()
    {
        if (!hexMouse.CursorIsOnMap) return;
        Vector3Int mouseTilePosition = hexMouse.TileCoord;
        HexTile<Tile> t = hexMap.TilesByPosition[mouseTilePosition];
        HoverTile(t);

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                SelectTile(t);
                SelectTileEvent?.Invoke(t);
            }
        }
    }

    public void HoverTile(HexTile<Tile> tile)
    {
        if (tileHover != null && tileHover != tile)
        {
            foreach (var highlightedUnit in highlightedUnits)
            {
                highlightedUnit.Highlight(false);
            }
            highlightedUnits.Clear();
            tileHover.Data.Hover(false);
        }

        tileHover = tile;
        tileHover.Data.Hover(true);
        if (tileHoverSfx) AudioManager.Instance.PlaySfx(tileHoverSfx);

        // Check if tile is in danger
        if (tileHover.Data.dangerList.Count != 0)
        {
            int totalDamage = 0;
            int enemyCount = 0;
            StringBuilder infoText = new StringBuilder();

            foreach (AIPlan aiPlan in tileHover.Data.dangerList)
            {
                enemyCount++;
                infoText.Append($"{aiPlan.actor.name}:");
                
                // highlight enemies attacking this tile
                aiPlan.actor.Highlight(true);
                highlightedUnits.Add(aiPlan.actor);

                if (aiPlan.ability.abilityEffects.Count != 0)
                {
                    foreach (AbilityEffect ability in aiPlan.ability.abilityEffects)
                    {
                        // Cast to understand what type of ability will hit
                        if (ability as DamageAbilityEffect)
                        {
                            DamageAbilityEffect damageAbility = ability as DamageAbilityEffect;
                            totalDamage += damageAbility.potency;

                            infoText.Append(($"{damageAbility.potency} dmg"));
                        }
                        //Other casts, if necessary(dotdamage etc..)
                        else if (ability as DamageOvertimeAbilityEffect)
                        {
                            DamageOvertimeAbilityEffect damageOvertimeAbility = ability as DamageOvertimeAbilityEffect;
                            totalDamage += damageOvertimeAbility.potency;

                            infoText.Append($"{damageOvertimeAbility.potency} Dot dmg ({damageOvertimeAbility.duration} turns)");
                        }
                        else if (ability as StatsAlteringAbilityEffect)
                        {
                            StatsAlteringAbilityEffect StatsAlteringAbility = ability as StatsAlteringAbilityEffect;
							
							infoText.Append(($", {StatsAlteringAbility.defense} DEF ({StatsAlteringAbility.duration} turns)"));

							infoText.AppendLine();
						}
                    }
                }
            }

            // Get and pass tileHover.Data.dangerList[0].actor and ability damage to UI
            forewarningController.ShowInfoText(infoText.ToString());

            // Show total damage done by enemies
            forewarningController.ShowTotalDamage(totalDamage);


            // check if there is a unit on the tile
            if (tileHover.Data.Unit != null)
            {
                // Show damage received by unit on the tile
                int unitDefense = tileHover.Data.Unit.defense;
                forewarningController.ShowDamageReceived(totalDamage - (unitDefense * enemyCount));
            }
            else
            {
                forewarningController.DamageReceivedPanel.SetActive(false);
            }

            // Visualize tab
            forewarningTab.SetActive(true);
        }
        else
        {
            forewarningTab.SetActive(false);
        }
    }

    public void SelectTile(HexTile<Tile> tile)
    {
        if (tileSelection != null && tileSelection != tile)
        {
            tileSelection.Data.Select(false);
        }

        tileSelection = tile;

        tileSelection.Data.Select(true);
        if (tileSelectionSfx) AudioManager.Instance.PlaySfx(tileSelectionSfx);
    }

    public void HighlightTiles(List<HexTile<Tile>> tiles)
    {
        foreach (var tile in tiles.Where(tile => tile != null))
        {
            tile.Data.Highlight(true);
        }
    }

    public GameObject SpawnUnit(GameObject obj, int index)
    {
        return SpawnEntity(obj, index, spawnUnitEffect);
    }

    public GameObject SpawnEnemy(GameObject obj, int index)
    {
        return SpawnEntity(obj, index, spawnEnemyEffect);
    }

    public GameObject SpawnEntity(GameObject obj, int index, GameObject spawnEffect)
    {
        HexTile<Tile> hexTile = hexMap.Tiles[index];
        if (unitPlacementSfx) AudioManager.Instance.PlaySfx(unitPlacementSfx);
        if (spawnEffect)
        {
            Instantiate(spawnEffect, hexTile.Data.transform);
        }

        return Instantiate(obj, hexTile.CartesianPosition, Quaternion.identity);
    }

    public IEnumerator GenerateBoard(LevelData levelData)
    {
        canvas.enabled = false;

        hexMap = new HexMap<Tile, int>(HexMapBuilder.CreateHexagonalShapedMap(levelData.boardRadius), null);
        hexMouse.Init(hexMap);
        tiles = new GameObject[hexMap.TilesByPosition.Count];
        edges = new GameObject[hexMap.EdgesByPosition.Count];

        var centerTile = hexMap.TilesByPosition[Vector3Int.zero];
        var centerPrototype = levelData.tiles.Find(ti => ti.index == centerTile.Index).proto;
        SpawnTile(centerPrototype, centerTile);
        for (int i = 1; i <= levelData.boardRadius; i++)
        {
            List<HexTile<Tile>> ring = hexMap.GetTiles.Ring(centerTile, i, 1);

            foreach (var tile in ring)
            {
                TilePrototype tilePrototype = levelData.tiles.Find(ti => ti.index == tile.Index).proto;
                SpawnTile(tilePrototype, tile);
                if (tilePlaceSfx) AudioManager.Instance.PlaySfx(tilePlaceSfx);
            }

            yield return new WaitForSeconds(0.2f);
        }


        if (edgePrefab)
        {
            foreach (var tileBorder in hexMap.Edges)
            {
                SpawnEdge(tileBorder);
            }
        }

        canvas.enabled = true;

        yield return null;
    }

    private void SpawnEdge(HexEdge<int> tileBorder)
    {
        GameObject instance = Instantiate(edgePrefab, transform);
        instance.name = "MapEdge_" + tileBorder.Position;
        instance.transform.position = tileBorder.CartesianPosition;
        instance.transform.rotation = Quaternion.Euler(0, tileBorder.EdgeAlignmentAngle, 0);
        edges[tileBorder.Index] = instance;
        instance.SetActive(true);
    }

    private void SpawnTile(TilePrototype tilePrototype, HexTile<Tile> tile)
    {
        GameObject instance = Instantiate(tilePrototype.prefab, transform);
        instance.transform.position = tile.CartesianPosition;
        instance.gameObject.name = "Hex" + tile.CartesianPosition + "[" + tile.Index + "]";
        tile.Data = instance.GetComponent<Tile>();
        tile.Data.board = this;
        tile.Data.pos = tile.Position;
        tile.Data.posCart = tile.CartesianPosition;
        tile.Data.posNorm = tile.NormalizedPosition;
        tile.Data.prototype = tilePrototype;
        tiles[tile.Index] = instance;

        if (animateTileSpawn)
        {
            var endPos = instance.transform.position;
            var startPos = instance.transform.position + Vector3.up * 2;
            StartCoroutine(EasePosition(instance.transform, startPos, endPos, 0.5f, EaseMethods.ElasticEaseOut));
        }
    }

    public IEnumerator EasePosition(Transform trans, Vector3 start, Vector3 end, float dur, Ease ease)
    {
        float t = 0f;
        while (t < dur)
        {
            float sc = ease(t, 0f, 1f, dur);
            trans.position = Vector3.LerpUnclamped(start, end, sc);

            yield return null;
            t += Time.deltaTime;
        }

        trans.position = end;
    }

    public List<HexTile<Tile>> SearchRange(HexTile<Tile> start, Func<HexTile<Tile>, HexTile<Tile>, bool> addTile,
        bool selectTilesAtEnd = false)
    {
        ClearPathfinding();

        List<HexTile<Tile>> tilesInRange = new List<HexTile<Tile>> {start};

        Queue<HexTile<Tile>> toCheck = new Queue<HexTile<Tile>>();
        toCheck.Enqueue(start);
        Queue<HexTile<Tile>> explored = new Queue<HexTile<Tile>>();
        start.Data._distance = 0;
        while (toCheck.Count > 0)
        {
            HexTile<Tile> current = toCheck.Dequeue();
            foreach (HexTile<Tile> next in hexMap.GetTiles.AdjacentToTile(current))
            {
                if (next == null || next.Data._distance <= current.Data._distance + 1)
                    continue;
                if (addTile(current, next))
                {
                    next.Data._distance = current.Data._distance + 1;
                    next.Data._prev = current;
                    explored.Enqueue(next);
                    tilesInRange.Add(next);
                }
            }

            SwapReference(ref toCheck, ref explored);
        }

        if (selectTilesAtEnd) HighlightTiles(tilesInRange);
        return tilesInRange;
    }

    private void ClearPathfinding()
    {
        foreach (HexTile<Tile> t in hexMap.Tiles)
        {
            t.Data._prev = null;
            t.Data._distance = int.MaxValue;
        }
    }

    void SwapReference(ref Queue<HexTile<Tile>> a, ref Queue<HexTile<Tile>> b)
    {
        (a, b) = (b, a);
    }


    public void ClearHighlight()
    {
        foreach (HexTile<Tile> hexMapTile in hexMap.Tiles)
        {
            hexMapTile.Data.Highlight(false);
        }
    }

    public List<Vector3Int> GetPathTiles(Vector3Int start, List<TileDirection> steps)
    {
        Vector3Int current = start;

        List<Vector3Int> runeTiles = new List<Vector3Int>();
        foreach (TileDirection step in steps)
        {
            runeTiles.Add(current + HexGrid.TileDirectionVectors[(int) step]);
            current += HexGrid.TileDirectionVectors[(int) step];
        }

        return runeTiles;
    }

    public void HighlightRune(List<HexTile<Tile>> tiles)
    {
        List<HexEdge<int>> hexEdges = hexMap.GetEdges.TileBorders(tiles);
        foreach (HexEdge<int> hexEdge in hexEdges)
        {
            MeshRenderer meshRenderer = edges[hexEdge.Index].GetComponentInChildren<MeshRenderer>();
            meshRenderer.material.color = Color.red;
        }
    }

    public List<Vector3Int> Reflect(List<Vector3Int> tiles, Vector3Int center, TileDirection direction)
    {
        List<Vector3Int> reflectedTiles = new List<Vector3Int>();
        foreach (var subPos in tiles.Select(pos => pos - center))
        {
            Vector3Int refPos;
            switch (direction)
            {
                case TileDirection.TopRight:
                case TileDirection.BottomLeft:
                    refPos = new Vector3Int(subPos.x, subPos.z, subPos.y) * new Vector3Int(-1, -1, -1);
                    break;
                case TileDirection.Right:
                case TileDirection.Left:
                    refPos = new Vector3Int(subPos.z, subPos.y, subPos.x) * new Vector3Int(-1, -1, -1);
                    break;
                case TileDirection.BottomRight:
                case TileDirection.TopLeft:
                    refPos = new Vector3Int(subPos.y, subPos.x, subPos.z) * new Vector3Int(-1, -1, -1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            reflectedTiles.Add(refPos + center);
        }

        return reflectedTiles;
    }

    public List<Vector3Int> RotateClockwise(List<Vector3Int> tiles, Vector3Int center)
    {
        return tiles.Select(hexTile => HexGrid.GetTile.FromTileRotated60DegreeClockwise(center, hexTile)).ToList();
    }

    public List<Vector3Int> RotateCounterClockwise(List<Vector3Int> tiles, Vector3Int center)
    {
        return tiles.Select(hexTile => HexGrid.GetTile.FromTileRotated60DegreeCounterClockwise(center, hexTile))
            .ToList();
    }

    public HexTile<Tile> GetTile(Vector3Int position)
    {
        HexTile<Tile> hexTile = null;
        try
        {
            hexTile = hexMap.TilesByPosition[position];
        }
        catch
        {
        }

        if (hexTile == null) return null;
        return hexTile;
    }
}