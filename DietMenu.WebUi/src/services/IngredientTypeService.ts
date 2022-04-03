import RestService from "./RestService";
import { GridRequest } from "@/models/Grid/GridRequest";
import { GridResponse } from "@/models/Grid/GridResponse";
import { IngredientType, IngredientTypes } from "@/models/IngredientType";

export default function IngredientTypeService() {
    const { post } = RestService();

    async function getIngredientTypes(): Promise<IngredientType[]> {
        const ingredientTypes = await post<IngredientTypes>("/api/IngredientTypes/GetIngredientTypes", {});

        return ingredientTypes.types;
    }

    async function getIngredientTypesGrid(gridRequest: GridRequest): Promise<GridResponse> {
        return await post<GridResponse>("/api/IngredientTypes/GetIngredientTypesGrid", { grid_request: gridRequest });
    }

    return {
        getIngredientTypes,
        getIngredientTypesGrid,
    }
}