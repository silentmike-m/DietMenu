import { GridRequest, GridResponse } from "~/types/core/grid";
import useRest from "./core/useRest";

export default function useMealTypes() {
    const { post } = useRest()

    async function getMealTypesGrid(request: GridRequest): Promise<GridResponse> {
        const response = await post<GridResponse>("MealTypes/GetMealTypesGrid", { grid_request: request });
        return response;
    }

    return {
        getMealTypesGrid
    }
}