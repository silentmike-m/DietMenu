import axios from "axios";
import { IBaseResponse } from "@/models/Rest/IBaseResponse";
import { RestError } from "@/models/Rest/RestError";
import AlertService from "./AlertService";
import { AlertType } from "@/models/Alert/AlertType";

export default function RestService() {
    const { showAlert } = AlertService();

    async function post<T>(path: string, data: any): Promise<T> {
        try {
            const headers = {
                "Content-Type": "application/json",
            };

            const response = await axios.post<IBaseResponse<T>>(path, JSON.stringify(data), { headers });
            return handleResponse(response.data);
        }
        catch (error: any) {
            return await handleError(error);
        }
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
        post,
    }
}