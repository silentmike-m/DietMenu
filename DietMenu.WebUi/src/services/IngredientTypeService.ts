import RestService from "./RestService";
import { GridRequest } from "@/models/Grid/GridRequest";
import { GridResponse } from "@/models/Grid/GridResponse";

export default function IngredientTypeService() {
    const { post } = RestService();

    async function getIngredientTypesGrid(gridRequest: GridRequest): Promise<GridResponse> {
        return await post<GridResponse>("/api/IngredientTypes/GetIngredientTypesGrid", { grid_request: gridRequest });
    }

    return {
        getIngredientTypesGrid
    }
}