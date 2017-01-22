using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : FireAnimationObject
{
    private GameController gameController;
    private EnemyController enemyController;
    private Renderer[] renderers;

    [SerializeField]
    private Color fullHealthColor;
    [SerializeField]
    private Color zeroHealthColor;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        enemyController = GetComponent<EnemyController>();
        List<Renderer> renderersList = new List<Renderer>(GetComponentsInChildren<MeshRenderer>());
        renderersList.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());
        renderers = renderersList.ToArray();
    }

    public override void SetHealthUI()
    {
        base.SetHealthUI();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.Lerp(fullHealthColor, zeroHealthColor, 1 - currentHealth / startingHealth);
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        enemyController.StopEngine();
        GetComponent<BoxCollider>().enabled = false;
        gameController.EnemyKilled(10);
    }
}
