import { Guid } from "guid-typescript";
import { GridRequest } from "../Grid/GridRequest";
import { Ingredient } from "./Ingredient";

export class DeleteIngredient {
    id: string = "";
}

export class GetIngredient {
    id: string = "";
}

export class GetIngredientsGrid {
    grid_request: GridRequest = new GridRequest();
    type_id: string | null = null;
}

export class GetReplacementsGrid {
    exchanger: number = 0;
    grid_request: GridRequest = new GridRequest();
    quantity: number = 0;
    type_id: string = Guid.createEmpty().toString();
}

export class UpsertIngredient {
    ingredient: Ingredient = new Ingredient();
}