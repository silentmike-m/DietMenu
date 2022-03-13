import { GridColumn } from "./grid"

export interface DialogOptions { }

export class InputDialogOptions {
    title: string = "";
    value: string = "";
    label: string = "Wartość";
    confirmText: string = "Zapisz";
    confirmAction: Function = () => { };
    cancelText: string = "Anuluj";
    cancelAction: Function = () => { };

    constructor(title: string, label: string, confirmAction: Function, cancelAction: Function) {
        this.title = title;
        this.label = label;
        this.confirmAction = confirmAction;
        this.cancelAction = cancelAction;
    }
}

export class GridDialogOptions implements DialogOptions {
    title: string
    multiSelect: Boolean
    getData: Function
    selectAction: Function
    cancelAction: Function
    columns: Array<GridColumn>
    initialFilter: string

    constructor(title: string, multiSelect: Boolean, getData: Function, selectAction: Function, cancelAction: Function, columns: Array<GridColumn>, initialFilter?: string) {
        this.title = title;
        this.multiSelect = multiSelect;
        this.getData = getData;
        this.selectAction = selectAction;
        this.cancelAction = cancelAction;
        this.columns = columns;
        this.initialFilter = initialFilter ?? "";
    }
}

export class YesNoDialogOptions implements DialogOptions {
    title: string
    question: string
    confirmAction: Function
    cancelAction: Function

    constructor(title: string, question: string, confirmAction: Function, cancelAction: Function) {
        this.title = title;
        this.question = question;
        this.confirmAction = confirmAction;
        this.cancelAction = cancelAction;
    }
}