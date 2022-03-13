import { useContext } from '@nuxtjs/composition-api'
import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { BaseResponse, RestError } from '~/types/core/rest'
import useAlert from './useAlert'

export default function useRest() {
    const { showError }: any = useAlert()
    const { app }: any = useContext()

    async function post<T>(path: string, data: any): Promise<T> {
        console.log(JSON.stringify(data));

        return await (app.$axios as NuxtAxiosInstance).post<BaseResponse<T>>(path, JSON.stringify(data), )
            .then(response => handleResponse(response.data))
            .catch((error) => handleError(error))
    }

    function applyToken(): AxiosRequestConfig {
        AxiosConf

        config.headers['Authorization'] = 'Bearer ' + authorizationState.getAccessToken();
        config.headers['Content-Type'] = 'application/json';
    }

    function handleError<T>(error: any): Promise<T> {
        if (error instanceof RestError) {
            showError(`${error.code}: ${error.message}`);
        }

        return Promise.reject();
    }

    function handleResponse<T>(response: BaseResponse<T>): T {
        if (response.code === "OK") {
            response.response;
        }

        throw new RestError(response.code, response.error ?? "");
    }


    return {
        post,
    }
}