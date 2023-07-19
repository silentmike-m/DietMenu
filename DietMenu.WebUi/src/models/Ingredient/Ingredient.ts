import { Guid } from "guid-typescript";

export class Ingredient {
    id: string = Guid.create().toString();
    exchanger: number = 0;
    is_system: boolean = false;
    name: string = "";
    type_id: string = Guid.createEmpty().toString();
    type_name = "";
    unit_symbol = "";
}