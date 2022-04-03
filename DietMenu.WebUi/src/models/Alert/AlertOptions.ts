import { AlertType } from "./AlertType";

export class AlertOptions {
    text: string = "";
    type: AlertType = AlertType.normal;

    constructor(text: string, type: AlertType) {
        this.text = text;
        this.type = type;
    }
}