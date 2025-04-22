using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopShelfChips : ShopShelfBase
{
    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
        playerResourceData.ChipsFoodAmount++;

    }

    public override void UndoBuy(PlayerResourceData playerResourceData)
    {
        base.UndoBuy(playerResourceData);
        playerResourceData.ChipsFoodAmount--;
    }
}
