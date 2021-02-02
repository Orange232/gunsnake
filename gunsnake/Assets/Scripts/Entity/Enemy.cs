﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Enemy : Entity
{
    public int maxHealth = -1;
    protected int health;

    public int damage = 1;
    public bool randomizeStartingVars;

    protected SpriteRenderer spriteRenderer;

    private bool strobing;
    private Material oldMat;
    [HideInInspector]
    public static Material whiteFlashMat; // set in GameHandler.cs

    protected override void Awake()
    {
        base.Awake();
        health = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        oldMat = spriteRenderer.material;

        EnemyManager.AddEnemy(this);
    }

    public abstract void EnemyTick(int tick);

    #region Health

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            StrobeWhite(1);
        }
    }
    
    public virtual void Die()
    {
        Debug.Log("I died!!");

        // temp
        //gameObject.SetActive(false);
        EnemyManager.RemoveEnemy(gameObject);
    }

    #endregion

    #region Movement

    // manhattan distance
    public GameObject GetClosestPlayerSegment()
    {
        GameObject closest = Player.body[0];
        float dist = Manhattan(transform.position, closest.transform.position);
        for (int i = 1; i < Player.body.Length; i++)
        {
            if (Manhattan(transform.position, Player.body[i].transform.position) < dist)
            {
                closest = Player.body[i];
                dist = Manhattan(transform.position, closest.transform.position);
            }
        }
        return closest;
    }

    public Vector3 GetDirectionToPlayer(bool shouldDiagonal)
    {
        Vector3 dir = Vector3.zero;
        Vector3 vecToPlayer = Player.body[0].transform.position - transform.position;
        // tiebreaker
        Vector3 vecToPlayerTwo = Player.body[1].transform.position - transform.position;

        float angle = Mathf.Atan2(vecToPlayer.y, vecToPlayer.x) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan2(vecToPlayerTwo.y, vecToPlayerTwo.x) * Mathf.Rad2Deg;

        if (angle < 0)  angle += 360;
        if (angle2 < 0) angle2 += 360;

        if ((!shouldDiagonal && angle % 90 == 45) || (shouldDiagonal && angle % 45 == 22.5))
            dir = DirectionFromAngle(shouldDiagonal, angle2);
        else
            dir = DirectionFromAngle(shouldDiagonal, angle);

        return dir;
    }

    private Vector3 DirectionFromAngle(bool shouldDiagonal, float angle)
    {
        Vector3 dir = Vector3.zero;

        if (!shouldDiagonal)
        {
            if (angle > 315)
                dir = Vector3.right;
            else if (angle > 225)
                dir = Vector3.down;
            else if (angle > 135)
                dir = Vector3.left;
            else if (angle > 45)
                dir = Vector3.up;
            else
                dir = Vector3.right;
        }
        else
        {
            if (angle > 337.5)
                dir = Vector3.right;
            else if (angle > 292.5)
                dir = new Vector3(1, -1);
            else if (angle > 247.5)
                dir = Vector3.down;
            else if (angle > 202.5)
                dir = new Vector3(-1, -1);
            else if (angle > 157.5)
                dir = Vector3.left;
            else if (angle > 112.5)
                dir = new Vector3(-1, 1);
            else if (angle > 67.5)
                dir = Vector3.up;
            else if (angle > 22.5)
                dir = new Vector3(1, 1);
            else
                dir = Vector3.right;
        }

        return dir;
    }

    #endregion


    #region Strobe Color

    protected void StrobeColor(int _strobeCount, Color _toStrobe)
    {
        if (strobing)
            return;

        strobing = true;

        StartCoroutine(StrobeColorHelper(0, (_strobeCount * 2) - 1));
    }

    protected void StrobeWhite(int _strobeCount)
    {
        StrobeColor(_strobeCount, Color.white);
    }

    private IEnumerator StrobeColorHelper(int _i, int _stopAt)
    {
        if (_i <= _stopAt)
        {
            if (_i % 2 == 0)
                spriteRenderer.material = whiteFlashMat;
            else
                spriteRenderer.material = oldMat;

            yield return new WaitForSeconds(0.125f);
            StartCoroutine(StrobeColorHelper((_i + 1), _stopAt));
        }
        else
        {
            strobing = false;
        }
    }

    #endregion
}
