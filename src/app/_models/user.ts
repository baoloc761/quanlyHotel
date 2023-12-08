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