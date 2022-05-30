import RestService from "./RestService";
import { GridResponse } from "@/models/Grid/GridResponse";
import { IngredientType, IngredientTypes } from "@/models/IngredientType/IngredientType";
import { GetIngredientTypes, GetIngredientTypesGrid } from "@/models/IngredientType/IngredientTypeRequests";

export default function IngredientTypeService() {
    const { post } = RestService();

    async function getIngredientTypes(request: GetIngredientTypes): Promise<IngredientType[]> {
        const ingredientTypes = await post<IngredientTypes>("/api/IngredientTypes/GetIngredientTypes", request);

        return ingredientTypes.types;
    }

    async function getIngredientTypesGrid(request: GetIngredientTypesGrid): Promise<GridResponse> {
        return await post<GridResponse>("/api/IngredientTypes/GetIngredientTypesGrid", request);
    }

    return {
        getIngredientTypes,
        getIngredientTypesGrid,
    }
}