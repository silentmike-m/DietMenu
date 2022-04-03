export class YesNoDialogOptions {
    title: string;
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