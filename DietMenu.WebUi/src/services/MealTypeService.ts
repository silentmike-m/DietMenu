import RestService from "./RestService";
import { GridRequest } from "@/models/Grid/GridRequest";
import { GridResponse } from "@/models/Grid/GridResponse";

export default function MealTypeService() {
    const { post } = RestService();

    async function getMealTypesGrid(gridRequest: GridRequest): Promise<GridResponse> {
        return await post<GridResponse>("/api/MealTypes/GetMealTypesGrid", { grid_request: gridRequest });
    }

    return {
        getMealTypesGrid
    }
}