import mealTypesState from "~/store/mealTypes";
import { GridRequest, GridResponse } from "~/types/core/grid";
import { MealType } from "~/types/mealTypes";
import useRest from "./core/useRest";

export default function useMealTypes() {
    const { post } = useRest()

    async function getMealTypes(): Promise<MealType[]> {
        let mealTypes = mealTypesState.get();

        if (mealTypes === undefined || mealTypes.length <= 0) {
            let gridRequest: GridRequest = {
                filter: "",
                order_by: "order",
                is_descending: false,
                is_paged: false,
                page_number: 0,
                page_size: 0,
            };
            await getMealTypesGrid(gridRequest).then((response) => {
                mealTypesState.set(response.elements);
                mealTypes = response.elements;
            })

            return mealTypes;
        }
        else {
            return mealTypes;
        }
    }

    async function getMealTypesGrid(request: GridRequest): Promise<GridResponse> {
        const response = await post<GridResponse>("MealTypes/GetMealTypesGrid", { grid_request: request });
        return response;
    }

    return {
        getMealTypes,
        getMealTypesGrid
    }
}