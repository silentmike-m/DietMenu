import { GridColumnType } from "./GridColumnType";

export class GridColumn {
    filterable: boolean;
    sortable: boolean;
    title: string;
    type: GridColumnType;
    value: string;

    constructor(filterable: boolean, sortable: boolean, title: string, type: GridColumnType, value: string) {
        this.value = value;
        this.title = title;
        this.type = type;
        this.sortable = sortable;
        this.filterable = filterable;
    }
}