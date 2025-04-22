using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class SurfaceSlot : MonoBehaviour
{
    public Transform itemTransform;
    public Vector3 itemPosition;
    public quaternion itemRotation;
    public GameObject itemInSlot;
    public GameObject slotGO;
    public Transform parentTransfrom;

    // Start is called before the first frame update
    void Start()
    {
        itemTransform = this.transform;
        itemPosition = itemTransform.position;
        itemRotation = itemTransform.rotation;

        slotGO = gameObject.transform.parent.gameObject;
        parentTransfrom = gameObject.transform.parent;
    }
}
