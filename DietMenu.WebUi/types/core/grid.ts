export interface GridColumn {
    value: string;
    text: string;
    type: GridColumnType;
    sortable: boolean;
    filterable: boolean;
}

export enum GridColumnType {
    text,
    number,
    date,
    boolean,
}

export interface GridMenuButton {
    id: number;
    text: string;
    icon: string;
    onClick: Function;
}

export class GridRequest {
    constructor(
        public filter: string,
        public is_descending: Boolean,
        public is_paged: Boolean,
        public order_by: string,
        public page_number: number,
        public page_size: number,
    ) { }
}

export class GridResponse {
    elements: any[] = [];
    count: number = 0;
}