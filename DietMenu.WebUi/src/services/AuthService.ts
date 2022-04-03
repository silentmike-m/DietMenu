import axios from "axios";
import { User } from "@/models/User";
import RestService from "./RestService";
import AlertService from "./AlertService";
import { AlertType } from "@/models/Alert/AlertType";
import { RestError } from "@/models/Rest/RestError";

export default function AuthService() {
    const { get, postWithToken } = RestService();

    async function getInformationAboutMySelf(): Promise<User> {
        const token = await get<string>("/getToken");
        console.log(token);
        return new User();

        // return await postWithToken<User>("https://localhost:30000/User/GetInformationAboutMyself", {}, token);
    }

    return {
        getInformationAboutMySelf,
    }
}