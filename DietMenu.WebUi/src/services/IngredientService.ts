import RestService from "./RestService";
import { GridRequest } from "@/models/Grid/GridRequest";
import { GridResponse } from "@/models/Grid/GridResponse";

export default function IngredientService() {
    const { post } = RestService();

    async function getIngredientsGrid(gridRequest: GridRequest, typeId: string | null): Promise<GridResponse> {
        return await post<GridResponse>("/api/Ingredients/GetIngredientsGrid", { grid_request: gridRequest, type_id: typeId });
    }

    return {
        getIngredientsGrid
    }
}