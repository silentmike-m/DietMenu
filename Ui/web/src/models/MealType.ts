export class MealType {
    id: string;
    name: string;
    order: number;

    constructor(id: string, name: string, order: number) {
        this.id = id;
        this.name = name;
        this.order = order;
    }
}