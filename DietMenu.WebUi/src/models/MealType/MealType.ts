import { Guid } from "guid-typescript";

export class MealType {
    id: string = Guid.create().toString();
    name: string = "";
    order: number = 1;
}