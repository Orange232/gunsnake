﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSegmentHealth : MonoBehaviour
{
    public PlayerHealth playerHealth;

    void Start()
    {

    }
    
    public void TakeDamage(int amount)
    {
        playerHealth.TakeDamage(amount);
    }

}
