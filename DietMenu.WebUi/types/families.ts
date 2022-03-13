export interface Family {
    id: string,
    name: string,
    users: FamilyUser[],
}

export interface FamilyUser {
    id: string,
    email: string,
    first_name: string,
    invitation_date: Date | null,
    last_name: string,
    status: string,
    user_name: string,
}