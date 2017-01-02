﻿using UnityEngine;
using System.Collections;

public class EnemyHealth : DestructableObject
{
    private GameController gameController;

    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private Color fullHealthColor;
    [SerializeField]
    private Color zeroHealthColor;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public override void SetHealthUI()
    {
        base.SetHealthUI();
        _renderer.material.color = Color.Lerp(fullHealthColor, zeroHealthColor, 1 - currentHealth / startingHealth);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        //TODO: real enemy value
        gameController.EnemyKilled(10);
        Destroy(gameObject);
    }
}
