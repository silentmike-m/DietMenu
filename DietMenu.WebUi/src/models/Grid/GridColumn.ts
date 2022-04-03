import { GridColumnType } from "./GridColumnType";

export class GridColumn {
    sortable: boolean;
    title: string;
    type: GridColumnType;
    value: string;

    constructor(sortable: boolean, title: string, type: GridColumnType, value: string) {
        this.value = value;
        this.title = title;
        this.type = type;
        this.sortable = sortable;
    }
}