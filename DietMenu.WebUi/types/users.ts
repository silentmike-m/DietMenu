export interface InvitedUser {
    first_name: string,
    last_name: string,
    password: string,
    user_name: string,
}

export interface UserToCreate {
    id: string,
    create_code: string,
    email: string,
    family_name: string,
    first_name: string,
    last_name: string,
    password: string,
    user_name: string,
}