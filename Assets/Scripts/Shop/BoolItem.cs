using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolItem : ShopItem
{
    public bool WasBought = false;

    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
    }

    public virtual void InitializeSelf(PlayerResourceData playerResourceData)
    {

    }
}
