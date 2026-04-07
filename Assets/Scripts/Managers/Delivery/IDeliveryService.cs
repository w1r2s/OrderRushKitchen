using System;
using System.Collections.Generic;

public interface IDeliveryService
{
    event EventHandler OnRecipeSpawned;
    event EventHandler OnRecipeCompleted;
    event EventHandler OnRecipeSuccess;
    event EventHandler OnRecipeFailed;

    void Tick(float deltaTime);
    void DeliverRecipe(PlateKitchenObject plateKitchenObject);

    List<ProcessRecipeSo> GetWaitingRecipes();
    int GetSuccessfulRecipesAmount();
}