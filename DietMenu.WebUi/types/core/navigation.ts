export class MenuElement {
    constructor(
        public name: string,
        public path: string,
        public iconClass: string,
        public childElements: MenuElement[],
    ) {
    }
}