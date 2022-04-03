export class GridRequest {
    constructor(
        public filter: string,
        public is_descending: boolean,
        public is_paged: boolean,
        public order_by: string,
        public page_number: number,
        public page_size: number,
    ) { }
}