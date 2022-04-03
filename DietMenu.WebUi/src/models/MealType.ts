import { Guid } from "guid-typescript";

export class MealType {
    id: Guid = Guid.create();
    name: string = "";
    order: number = 1;
}