import { Family, FamilyUser } from "~/types/families";
import useRest from "./core/useRest";

export default function useFamily() {
    const { post } = useRest()

    async function getFamily(id: string): Promise<Family> {
        const response = await post<Family>("Family/GetFamily", { id: id });
        return response;
    }

    async function inviteUser(user: FamilyUser): Promise<boolean> {
        try {
            await post("User/InviteUser", { email: user.email, id: user.id });
            return true;
        } catch (e) {
            return false;
        }
    }

    return {
        getFamily,
        inviteUser,
    }
}