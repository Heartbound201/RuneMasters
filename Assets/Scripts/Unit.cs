using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class Unit : BoardObject, IDamageable
{
    public string name;
    public int movement;
    public int movementMax;
    public bool hasActed;
    public bool hasMoved;
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

    public Animator animator;
    public SkinnedMeshRenderer meshRenderer;
    public Color originalColor;

	public float positionY = 0f;

	[Header("Audio")] public AudioClipSO getHitSfx;
    public AudioClipSO deathSfx;
    public AudioClipSO attackSfx;

	protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        strengthStart = strength;
        defenseStart = defense;
        intelligenceStart = intelligence;
        dexterityStart = dexterity;
        originalColor = meshRenderer.material.color;

		Vector3 translatePosition = new Vector3(transform.position.x, positionY, transform.position.z);
		transform.position = translatePosition;
	}

    public virtual void PlaceOnTile(HexTile<Tile> target)
    {
        // Make sure old tile location is not still pointing to this unit
        if (tile != null && tile.Data.content.Contains(this))
            tile.Data.content.Remove(this);

        // Link unit and tile references
        tile = target;

        if (target != null)
        {
            target.Data.content.Add(this);
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
        return (from.Data._distance + 1) <= movement && to.Data.IsPassable;
    }

    public virtual IEnumerator MoveTact(List<HexTile<Tile>> tiles)
    {
        for (int i = 1; i < tiles.Count; ++i)
        {
            yield return StartCoroutine(Turn(tiles[i]));
            yield return StartCoroutine(Walk(tiles[i]));
            PlaceOnTile(tiles[i]);

            movement = Mathf.Clamp(movement - 1, 0, movementMax);
        }

        hasMoved = true;

        if (animator) animator.Play("Idle");

        yield return null;
    }

    public virtual IEnumerator MoveRune(List<HexTile<Tile>> tiles)
    {
        foreach (HexTile<Tile> to in tiles)
        {
            yield return StartCoroutine(Turn(to));
            yield return StartCoroutine(Walk(to));
            PlaceOnTile(to);

            movement = Mathf.Clamp(movement - 1, 0, movementMax);
        }

        if (animator) animator.Play("Idle");

        yield return null;
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
            if (tiles[i].Data.content.Count > 0)
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

    public virtual void Reset()
    {
        movement = movementMax;
        hasActed = false;
        hasMoved = false;
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
        if (animator) animator.Play("GetHit");
        if (getHitSfx) AudioManager.Instance.PlaySfx(getHitSfx);
    }

    public virtual int AvailableMana()
    {
        return movement;
    }

    public virtual void Heal(int amount)
    {
    }

    public virtual IEnumerator Walk(HexTile<Tile> tile)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;
        float waitTime = 0.5f;

        if (animator)
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
        if (direction == dir) yield break;
        Quaternion startRot = transform.rotation;
        float elapsedTime = 0f;
        float waitTime = 0.15f;

        if (animator) animator.Play("Idle");

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

    public virtual IEnumerator Turn(HexTile<Tile> targetTile)
    {
        yield return Turn(tile.GetDirection(targetTile));
    }

    public IEnumerator Act(Ability ability, HexTile<Tile> targetTile)
    {
        if (ability == null) yield break;
        yield return Turn(targetTile);

        if (animator) animator.Play("Attack");
        if (attackSfx) AudioManager.Instance.PlaySfx(attackSfx);

        yield return ability.Execute(this, targetTile);

        hasActed = true;

        if (animator) animator.Play("Idle");

        yield return new WaitForSeconds(1f);
    }

    protected virtual void Die()
    {
        if (animator) animator.Play("Die");
        if (deathSfx) AudioManager.Instance.PlaySfx(deathSfx);
    }

    public virtual void Highlight(bool value)
    {
        try
        {
            if (value)
            {
                meshRenderer.material.color = Color.red;
            }
            else
            {
                meshRenderer.material.color = originalColor;
            }
        }
        catch (Exception ignored)
        {
            // ignoring shaders that don't expose _color
        }

        
    }
}