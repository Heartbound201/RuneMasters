﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class Unit : MonoBehaviour
{
    public string name;
    public int movement;
    public int movementMax;
    public bool isPassable;
    public bool hasActed;
    public HexTile<Tile> tile;
    public TileDirection direction;
    public List<Status> statuses = new List<Status>();

    public int strength;
    public int defense;
    public int intelligence;
    public int dexterity;

    [HideInInspector] public int strengthStart;
    [HideInInspector] public int defenseStart;
    [HideInInspector] public int intelligenceStart;
    [HideInInspector] public int dexterityStart;

    protected Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        strengthStart = strength;
        defenseStart = defense;
        intelligenceStart = intelligence;
        dexterityStart = dexterity;
    }

    public virtual void PlaceOnTile(HexTile<Tile> target)
    {
        // Make sure old tile location is not still pointing to this unit
        if (tile != null && tile.Data.unitList.Contains(this))
            tile.Data.unitList.Remove(this);

        // Link unit and tile references
        tile = target;

        if (target != null)
        {
            target.Data.unitList.Add(this);
            transform.position = target.CartesianPosition;
        }
    }

    public virtual void PlaceOnTile(HexTile<Tile> target, TileDirection tileDirection)
    {
        PlaceOnTile(target);
        this.direction = tileDirection;
        transform.rotation = Quaternion.Euler(tileDirection.ToEuler());
    }

    public virtual bool ExpandSearch(HexTile<Tile> from, HexTile<Tile> to)
    {
        return (from.Data._distance + 1) <= movement && to.Data.isPassable;
    }

    public virtual IEnumerator Move(List<HexTile<Tile>> tiles)
    {
        for (int i = 1; i < tiles.Count; ++i)
        {
            TileDirection tileDirection = tile.GetDirection(tiles[i]);
            if (direction != tileDirection)
                yield return StartCoroutine(Turn(tileDirection));
            yield return StartCoroutine(Walk(tiles[i]));
            PlaceOnTile(tiles[i]);

            movement = Mathf.Clamp(movement - 1, 0, movementMax);
            yield return null;
        }

        if (animator != null)
        {
            animator.Play("Idle");
        }
    }

    public virtual IEnumerator MoveRune(List<HexTile<Tile>> tiles)
    {
        foreach (HexTile<Tile> to in tiles)
        {
            TileDirection tileDirection = tile.GetDirection(to);
            if (direction != tileDirection)
                yield return StartCoroutine(Turn(tileDirection));
            yield return StartCoroutine(Walk(to));
            PlaceOnTile(to);

            movement = Mathf.Clamp(movement - 1, 0, movementMax);
            yield return null;
        }

        if (animator != null)
        {
            animator.Play("Idle");
        }
    }

    public virtual List<HexTile<Tile>> GetTilesInRange()
    {
        List<HexTile<Tile>> retValue = tile.Data.board.SearchRange(tile, ExpandSearch);
        Filter(retValue);
        return retValue;
    }

    protected virtual void Filter(List<HexTile<Tile>> tiles)
    {
        for (int i = tiles.Count - 1; i > 0; --i)
            if (tiles[i].Data.unitList.Count > 0)
                tiles.RemoveAt(i);
    }

    public virtual List<HexTile<Tile>> FindPath(HexTile<Tile> tile)
    {
        List<HexTile<Tile>> targets = new List<HexTile<Tile>>();
        while (tile != null)
        {
            targets.Insert(0, tile);
            tile = tile.Data._prev;
        }

        return targets;
    }

    public void Reset()
    {
        movement = movementMax;
        hasActed = false;
    }

    public void TriggerStatusStart()
    {
        foreach (var status in statuses)
        {
            status.ApplyOnTurnStart(this);
        }
    }

    public void TriggerStatusEnd()
    {
        for (int i = statuses.Count - 1; i >= 0; i--)
        {
            statuses[i].ApplyOnTurnEnd(this);
        }
    }

    public virtual void TakeDamage(int amount)
    {
        if (animator != null)
        {
            animator.Play("GetHit");
        }
    }

    public virtual void Heal(int amount)
    {
    }

    public virtual IEnumerator Walk(HexTile<Tile> tile)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;
        float waitTime = 0.5f;

        if (animator != null)
        {
            animator.Play("Run");
        }

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(startPos, tile.CartesianPosition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = tile.CartesianPosition;
        yield return null;
    }


    public virtual IEnumerator Turn(TileDirection dir)
    {
        Quaternion startRot = transform.rotation;
        float elapsedTime = 0f;
        float waitTime = 0.5f;

        if (animator != null)
        {
            animator.Play("Idle");
        }

        while (elapsedTime < waitTime)
        {
            transform.rotation = Quaternion.Lerp(startRot, Quaternion.Euler(dir.ToEuler()), (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(dir.ToEuler());
        direction = dir;
        yield return null;
    }
}