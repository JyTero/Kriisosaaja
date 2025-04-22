using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShopPortableStoveItem : BoolItem
{
    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
        playerResourceData.GotPortableStove = true;
    }
    public override void InitializeSelf(PlayerResourceData playerResourceData)
    {
        base.InitializeSelf(playerResourceData);
        
        if(playerResourceData.GotPortableStove)
        {
            this.gameObject.SetActive(false);
        }
    }
}
