import RestService from "./RestService";
import { GridResponse } from "@/models/Grid/GridResponse";
import { GetMealTypesGrid } from "@/models/MealType/MealTypeRequests";

export default function MealTypeService() {
    const { post } = RestService();

    async function getMealTypesGrid(request: GetMealTypesGrid): Promise<GridResponse> {
        return await post<GridResponse>("/api/MealTypes/GetMealTypesGrid", request);
    }

    return {
        getMealTypesGrid
    }
}