using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;

    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSo recipeListSo;

    private List<RecipeSo> waitingRecipeSoLitst;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;

    private void Awake()
    {
        Instance = this;

        waitingRecipeSoLitst = new List<RecipeSo>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (waitingRecipeSoLitst.Count < waitingRecipeMax)
            {
                RecipeSo waitingRecipeSo = recipeListSo.recipeSoList[UnityEngine.Random.Range(0, recipeListSo.recipeSoList.Count)];
                waitingRecipeSoLitst.Add(waitingRecipeSo);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSoLitst.Count; i++)
        {
            RecipeSo waitingRecipeSo = waitingRecipeSoLitst[i];
            if(waitingRecipeSo.kitchenObjectSoList.Count == plateKitchenObject.GetKitchenObjectSoList().Count)
            {
                bool plateMatch = true;
                foreach (KitchenObjectSo recipeKitchenObjectSo in waitingRecipeSo.kitchenObjectSoList)
                {
                    bool found = false;
                    foreach (KitchenObjectSo plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectSoList())
                    {
                        if(plateKitchenObjectSo == recipeKitchenObjectSo)
                        {
                            found = true;
                            break;
                        }
                    }
                    if(!found)
                    {
                        plateMatch = false;
                    }
                }
                if(plateMatch)
                {
                    waitingRecipeSoLitst.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        //wrong recipe
    }

    public List<RecipeSo> GetWaitingRecipeSoLits()
    {
        return waitingRecipeSoLitst;
    }
}