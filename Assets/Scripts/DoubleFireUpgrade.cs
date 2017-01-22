using UnityEngine;
using System.Collections;

public class DoubleFireUpgrade : Upgrade
{
    [SerializeField]
    private GameObject defaultWeapon;

    public override void Select()
    {
        base.Select();
        defaultWeapon.SetActive(false);
    }

    public override void Deselect()
    {
        base.Deselect();
        defaultWeapon.SetActive(true);
    }
}
