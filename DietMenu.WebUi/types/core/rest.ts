export interface BaseResponse<T> {
    code: string,
    error: string,
    response: T
}

export class RestError extends Error {
    constructor(
        public code: string,
        public message: string,
    ) {
        super(message)
    }
}