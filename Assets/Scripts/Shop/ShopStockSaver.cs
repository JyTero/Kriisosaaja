using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopStockSaver : MonoBehaviour
{
    [HideInInspector]
    public bool SaveComplete = false;

    [SerializeField]
    private List<ShopShelfBase> sceneShelves;
    [SerializeField]
    private GameObject boolItemParent;

    private bool debug = false;

    public void SaveShopState()
    {
        //Dictionary<int,int> into which shelves are added in order they are in the scenShelves as index and stock as value
        GlobalValues.ShopStockLeft = new();
        foreach (ShopShelfBase sceneShelf in sceneShelves)
        {
            GlobalValues.ShopStockLeft.Add(sceneShelves.IndexOf(sceneShelf), sceneShelf.stockLeft);
        }

        GlobalValues.AvailableShopBoolItems = new();
        foreach (Transform t in boolItemParent.transform)
        {
            if (t.gameObject.activeSelf)
            {
                GlobalValues.AvailableShopBoolItems.Add(t.gameObject);
            }
        }
        SaveComplete = true;
        Debug.Log("SHOP SAVED!");
    }

    //Used when exiting the scene and returning home
    public void SaveAndChangeScene()
    {
        StartCoroutine(SaveShopStateCoro());
    }
    public IEnumerator SaveShopStateCoro()
    {

        SaveShopState();
        yield return new WaitUntil(() => SaveComplete);
        ChangeScene();
    }
    public void LoadShopState()
    {
        //"Load" and apply shelf stocks
        foreach (var dictionaryEntry in GlobalValues.ShopStockLeft)
        {
            int sceneShelvesIndex = dictionaryEntry.Key;

            //Go through all shelves, apply values when matchin shelf found and continue to the next shelf
            foreach (ShopShelfBase sceneShelf in sceneShelves)
            {
                if (sceneShelvesIndex == sceneShelves.IndexOf(sceneShelf))
                {
                    sceneShelf.stockLeft = dictionaryEntry.Value;
                    goto nextSavedShelf;
                }
            }
            nextSavedShelf:;
        }

        //"Load" and apply bool item 
        foreach (Transform t in boolItemParent.transform)
        {
            foreach (GameObject go in GlobalValues.AvailableShopBoolItems)
            {
                if (t.gameObject == go)
                {
                    t.gameObject.SetActive(true);
                    //No need to continue the inner loop, continue the outer loop
                    goto nextBoolItem;
                }
            }
            nextBoolItem:;
        }
        if (debug)
            Debug.Log("SHOP LOADED!");
    }
    private void ChangeScene()
    {
        if (debug)
            Debug.Log("SCENE CHANGE!");
        SceneManager.LoadSceneAsync("HomeSceneWrking");
    }
}
