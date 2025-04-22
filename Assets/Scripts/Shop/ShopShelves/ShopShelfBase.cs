using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;
[System.Serializable]
public abstract class ShopShelfBase : ShopItem
{
    public int stockLeft;

    protected override void Start()
    {
        if (stockLeft <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);

        stockLeft--;
        if (stockLeft <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
