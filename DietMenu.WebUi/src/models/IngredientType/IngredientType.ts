import { Guid } from "guid-typescript";

export class IngredientType {
    id: string = Guid.create().toString();
    name: string = "";
}

export class IngredientTypes {
    types: IngredientType[] = [];
}