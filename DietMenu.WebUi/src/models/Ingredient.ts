import { Guid } from "guid-typescript";

export class Ingredient {
    id: Guid = Guid.create();
    exchanger: number = 0;
    is_system: boolean = false;
    name: string = "";
    type_id = Guid.createEmpty();
    type_name = "";
    unit_symbol = "";
}