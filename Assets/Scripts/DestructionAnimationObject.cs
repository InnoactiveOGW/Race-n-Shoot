using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DestructionAnimationObject : DestructableObject
{
    [SerializeField]
    private GameObject normal;
    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private Animation deathAnimation;

    public override void OnDeath()
    {
        if (explosion != null && normal != null)
        {
            explosion.SetActive(true);
            normal.SetActive(false);
        }

        deathAnimation[deathAnimation.clip.name].speed = 2;
        deathAnimation.Play();
    }
}