export class User {
    id?: string;
    username?: string;
    password?: string;
    firstName?: string;
    lastName?: string;
    typeUser?: { id: number, name: string, isAdmin: boolean };
    token?: string;
}