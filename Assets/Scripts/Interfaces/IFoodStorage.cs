using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFoodStorage 
{
    public List<SurfaceSlot> SurfaceSlots { get; set; }
    public Transform SlotParentT { get; set; }

    public void LoadItemToSlot(GameObject item, int slotIndex, bool spoiled, int spoilTime);
    public void FoodLeftCheck(PlayerResourceData playerResourseData);
}

