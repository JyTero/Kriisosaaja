using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRadioItem : BoolItem
{
    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
        playerResourceData.GotRadio = true;
    }
    public override void InitializeSelf(PlayerResourceData playerResourceData)
    {
        base.InitializeSelf(playerResourceData);

        if (playerResourceData.GotRadio)
        {
            this.gameObject.SetActive(false);
        }
    }
}
