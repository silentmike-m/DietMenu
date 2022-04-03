export interface IBaseResponse<T> {
    code: string,
    error: string,
    response: T
}
