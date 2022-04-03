import { Guid } from "guid-typescript";

export class IngredientType {
    id: Guid = Guid.create();
    name: string = "";
}

export class IngredientTypes {
    types: IngredientType[] = [];
}