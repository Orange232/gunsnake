﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _body = new GameObject[4];
    public static GameObject[] body = new GameObject[4];
    [SerializeField]
    private SpriteRenderer[] _sprites = new SpriteRenderer[4];
    public static SpriteRenderer[] sprites = new SpriteRenderer[4];

    [SerializeField]
    private PlayerHealth _playerHealth;
    public static PlayerHealth playerHealth;
    [SerializeField]
    private PlayerMovement _playerMovement ;
    public static PlayerMovement playerMovement;
    [SerializeField]
    private PlayerWeaponManager _playerWeaponManager;
    public static PlayerWeaponManager playerWeaponManager;

    private void Awake()
    {
        body = _body;
        sprites = _sprites;
        playerHealth = _playerHealth;
        playerMovement = _playerMovement;
        playerWeaponManager = _playerWeaponManager;

        TimeTickSystem.OnTick_PlayerMove += TimeTickSystem_OnTick;
    }

    // my favorite method
    public GameObject GetHead()
    {
        return body[0];
    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        playerHealth.OnTick(e.tick);
        playerMovement.OnTick(e.tick);
    }
}
