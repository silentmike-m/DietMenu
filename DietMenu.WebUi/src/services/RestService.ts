import axios from "axios";
import { IBaseResponse } from "@/models/Rest/IBaseResponse";
import { RestError } from "@/models/Rest/RestError";
import AlertService from "./AlertService";
import { AlertType } from "@/models/Alert/AlertType";

export default function MealTypeService() {
    const { showAlert } = AlertService();

    async function get<T>(path: string): Promise<T> {
        const headers = {
            "Content-Type": "text/plain",
        };

        return await axios.get<T>(path)
            .then(({ data }) => data)
            .catch((error) => handleError(error))
    }

    async function post<T>(path: string, data: any): Promise<T> {
        const headers = {
            "Content-Type": "application/json",
        };

        return await axios.post<IBaseResponse<T>>(path, JSON.stringify(data), { headers })
            .then(response => handleResponse(response.data))
            .catch((error) => handleError(error))
    }

    async function postWithToken<T>(path: string, data: any, token: string): Promise<T> {
        const headers = {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        };

        return await axios.post<IBaseResponse<T>>(path, JSON.stringify(data), { headers })
            .then(response => handleResponse(response.data))
            .catch((error) => handleError(error))
    }

    function handleError<T>(error: any): Promise<T> {
        const message = error instanceof RestError
            ? `${error.code}: ${error.message}`
            : error.message;

        showAlert({
            text: message,
            type: AlertType.error,
        });

        return Promise.reject();
    }

    function handleResponse<T>(response: IBaseResponse<T>): T {
        if (response.code !== "OK") {
            throw new RestError(response.code, response.error ?? "");
        }

        return response.response;
    }

    return {
        get,
        post,
        postWithToken,
    }
}