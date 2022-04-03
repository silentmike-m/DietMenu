import axios from "axios";
import { User } from "@/models/User";
import AlertService from "./AlertService";
import { AlertType } from "@/models/Alert/AlertType";

export default function AuthService() {
    const { showAlert } = AlertService();

    async function getInformationAboutMySelf(): Promise<User> {
        try {
            const token = await fetch("https://localhost:8080/getToken");
            const tokenString = await token.text();

            const headers = {
                "Authorization": `Bearer ${tokenString}`,
                "Content-Type": "application/json",
            };

            const response = await axios.post<User>("https://localhost:30000/User/GetInformationAboutMyself", {}, { headers });

            return response.data;
        }
        catch (error: any) {
            return await handleError(error);
        }
    }

    function handleError<T>(error: any): Promise<T> {
        showAlert({
            text: error.message,
            type: AlertType.error,
        });

        return Promise.reject();
    }

    return {
        getInformationAboutMySelf,
    }
}