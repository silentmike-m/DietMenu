// import { useContext } from '@nuxtjs/composition-api'
// import { InvitedUser, UserToCreate } from '~/types/users';
// import useAlert from './core/useAlert';
// import useRest from './core/useRest';

// export default function useUsers() {
//     const { showSuccess } = useAlert();
//     const { redirect }: any = useContext()
//     const { post } = useRest()

//     function changePassword(newPassword: string, oldPassword: string) {
//         post("User/ChangePassword", { new_password: newPassword, old_password: oldPassword }).then((response) => {
//             if (response) {
//                 redirect("/");
//             }
//         });
//     }

//     async function createInvitedUser(user: InvitedUser, token: string) {
//         await post<string>("User/CreateInvitedUser", { invitation_token: token, user: user }).then((response) => {
//             if (response) {
//                 redirect("/");
//             }
//         })
//     }

//     async function createUser(userToCreate: UserToCreate) {
//         await post<string>("User/CreateUser", { user: userToCreate }).then((response) => {
//             if (response) {

//                 redirect("/");
//             }
//         })
//     }

//     function login(user: string, pass: string) {
//         post<string>('User/Login', { user_name: user, password: pass }).then((response) => {
//             if (response) {
//                 redirect("/");
//             }
//         });
//     }

//     function logOut() {
//         redirect("/auth/login")
//     }

//     async function sendResetPasswordToken(email: string): Promise<boolean> {
//         try {
//             await post("User/SendResetPasswordToken", { email: email });
//             return true;
//         } catch (e) {
//             return false;
//         }
//     }

//     async function resetPassword(email: string, password: string, token: string) {
//         try {
//             await post("User/ResetPassword", { email: email, password: password, token: token });
//             redirect("/auth/login");
//             showSuccess("Zresetowano hasło");
//         } catch (e) {
//             redirect("/auth/login");
//         }
//     }

//     return {
//         changePassword,
//         createInvitedUser,
//         createUser,
//         login,
//         logOut,
//         sendResetPasswordToken,
//         resetPassword,
//     }
// }