import RestService from "./RestService";
import { GridResponse } from "@/models/Grid/GridResponse";
import { Ingredient } from "@/models/Ingredient/Ingredient";
import { DeleteIngredient, GetIngredient, GetIngredientsGrid, GetReplacementsGrid, UpsertIngredient } from "@/models/Ingredient/IngredientRequests";

export default function IngredientService() {
    const { post } = RestService();

    async function deleteIngredient(request: DeleteIngredient) {
        await post<any>("/api/Ingredients/DeleteIngredient", request);
    }

    async function getIngredient(request: GetIngredient): Promise<Ingredient> {
        return await post<Ingredient>("/api/Ingredients/GetIngredient", request);
    }

    async function getIngredientsGrid(request: GetIngredientsGrid): Promise<GridResponse> {
        return await post<GridResponse>("/api/Ingredients/GetIngredientsGrid", request);
    }

    async function getReplacementsGrid(request: GetReplacementsGrid): Promise<GridResponse> {
        return await post<GridResponse>("/api/Ingredients/GetReplacementsGrid", request);
    }

    async function upsertIngredient(request: UpsertIngredient): Promise<boolean> {
        return await post<boolean>("/api/Ingredients/UpsertIngredient", request);
    }

    return {
        deleteIngredient,
        getIngredient,
        getIngredientsGrid,
        getReplacementsGrid,
        upsertIngredient
    }
}