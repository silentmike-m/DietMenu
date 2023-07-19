import { Guid } from "guid-typescript"

export class Replacement {
    id: string = Guid.create().toString();
    exchanger: number = 0;
    name: string = "";
    quantity: number = 0;
    unit_symbol: string = "";
}