﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : Projectile
{
    public Vector3 direction;

    //  For making gifs
    //private void Awake()
    //{
    //    TimeTickSystem.OnTick_Projectiles += TimeTickSystem_OnTick;
    //}

    //private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    //{
    //    if (e.tick % 4 == 0)
    //        ProjectileTick(e.tick);
    //}

    public override void ProjectileTick(int tick)
    {
        if (tick % 2 == 0)
            transform.position += direction;
    }

    public override void SetValues(Projectile other)
    {
        baseDamage = other.baseDamage;
        basePierce = other.basePierce;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            e.TakeDamage(CalculateDamage(), direction);
            basePierce -= 1;
            if (basePierce < 0) 
                ProjectileManager.RemoveProjectile(gameObject);
        }
        if (other.tag == "Wall")
        {
            ProjectileManager.RemoveProjectile(gameObject);
        }
    }
}
