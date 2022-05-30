export class GridRequest {
    filter: string = "";
    is_descending: boolean = false;
    is_paged: boolean = true;
    order_by: string = "";
    page_number: number = 0;
    page_size: number = 10;
}