export class User {
    id?: string;
    username?: string;
    password?: string;
    firstName?: string;
    lastName?: string;
    typeUser?: { id: number, name: string, isAdmin: boolean };
    claims?: { 
        pageId: string,
        namePages: string, 
        canList: boolean, 
        canEdit: boolean, 
        canDelete: boolean, 
        canView: boolean, 
        canViewNamePage: boolean }[];
    token?: string;
}

export interface RoleDTO {
    id: string;
    type: number;
    name: string;
    description: string;
}

export interface LoginDTO {
    userName: string;
    password: string;
}

export interface RegistUserDTO {
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
    password: string;
}