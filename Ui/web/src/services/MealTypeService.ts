import { GridRequest } from "@/models/Grid/GridRequest";
import { GridResponse } from "@/models/Grid/GridResponse";
import { Guid } from "guid-typescript";
import { MealType } from "@/models/MealType";

export default function MealTypeService() {
    async function getMealTypesGrid(gridRequest: GridRequest): Promise<GridResponse> {
        console.log(gridRequest);

        const elements: MealType[] = [];

        for (let i = 1; i <= 100; i++) {
            const mealType = new MealType(Guid.create().toString(), `name ${i}`, i);

            elements.push(mealType);
        }

        const response = new GridResponse();
        response.count = 100;
        response.elements = elements.slice(gridRequest.page_number * gridRequest.page_size, gridRequest.page_number * gridRequest.page_size + gridRequest.page_size);

        return response;
    }

    return {
        getMealTypesGrid
    }
}