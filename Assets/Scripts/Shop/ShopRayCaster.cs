using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopRayCaster : MonoBehaviour
{
    public TextMeshProUGUI ItemDescription;
    public TextMeshProUGUI ItemPriceStock;

    private ShopManager shopManager;

    private void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
        ItemDescription.text = "";
        ItemPriceStock.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Ray is made on mouse pos
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //If UI hit
            if (EventSystem.current.currentSelectedGameObject == null)
            {

                if (Physics.Raycast(ray, out hit))
                {
                    ShopItem target = hit.collider.gameObject.GetComponent<ShopItem>();
                    //what is done when the raycast hits trigger
                    if (target != null)
                    {
                        Debug.Log("Ray hit: " + hit.collider.name);

                        //shopManager.shopNotifTextField.transform.parent.gameObject.SetActive(true);
                        shopManager.shopNotifTextField.gameObject.SetActive(true);
                        shopManager.shopNotifTextField.text = "";

                        //write the items description in the designated box
                        ItemDescription.text = target.ItemDescription;
                        string price = $"{target.ShopPrice} € / piece\n";
                        if (target is ShopShelfBase)
                        {
                            var t = target as ShopShelfBase;
                            price += $"Stock: {t.stockLeft} left";
                        }
                        ItemPriceStock.text = price;

                        //Enable purchase confirmation dialog
                        shopManager.purchaseConfirmation.gameObject.SetActive(true);
                        shopManager.purchaseConfirmation.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text
                            = "Buy " + target.ItemName + "?";
                        //Save item to Shop manager to handle purchase
                        shopManager.SelectedItemGO = target.gameObject;

                    }
                }
                else
                {
                    shopManager.purchaseConfirmation.gameObject.SetActive(false);
                    ItemDescription.text = "";
                    ItemPriceStock.text = "";
                }
            }
            else
            {
                ItemDescription.text = "";
                ItemPriceStock.text = "";
            }
        }
    }
}