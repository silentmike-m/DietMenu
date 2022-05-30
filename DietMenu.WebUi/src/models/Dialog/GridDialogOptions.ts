import { GridColumn } from "../Grid/GridColumn"

export class GridDialogOptions {
    title: string
    canMultiSelect: Boolean
    getGridData: Function
    selectAction: Function
    cancelAction: Function
    columns: Array<GridColumn>
    initialFilter: string

    constructor(title: string, canMultiSelect: Boolean, getGridData: Function, selectAction: Function, cancelAction: Function, columns: Array<GridColumn>, initialFilter?: string) {
        this.title = title;
        this.canMultiSelect = canMultiSelect;
        this.getGridData = getGridData;
        this.selectAction = selectAction;
        this.cancelAction = cancelAction;
        this.columns = columns;
        this.initialFilter = initialFilter ?? "";
    }
}