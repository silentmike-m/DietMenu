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