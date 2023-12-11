import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '@environments/environment';
import { RoleDTO, User } from '@app/_models';

// array in local storage for registered users

@Injectable({ providedIn: 'root' })
export class AccountService {
    private userSubject: BehaviorSubject<any | null>;
    public user: Observable<any | null>;
    public userApiUrl: string = `${environment.apiUrl}Login/`;

    constructor(
        private router: Router,
        private http: HttpClient
    ) {
        this.userSubject = new BehaviorSubject(JSON.parse(localStorage.getItem('user')!));
        this.user = this.userSubject.asObservable();
    }

    public get userValue() {
        return this.userSubject.value;
    }

    login(username: string, password: string) {
        return this.http.post<User>(`${environment.apiUrl}/users/authenticate`, { username, password })
            .pipe(map(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('user', JSON.stringify(user));
                this.userSubject.next(user);
                return user;
            }));
    }

    logout() {
        // remove user from local storage and set current user to null
        localStorage.removeItem('user');
        this.userSubject.next(null);
        this.router.navigate(['/account/login']);
    }

    register(user: User) {
        return this.http.post(`${environment.apiUrl}/users/register`, user);
    }

    updateUser(user: User) {
        return this.http.post(`${environment.apiUrl}/users/edit`, {user});
    }

    getListPages() {
        const listPages = [
            {
                id: 1,
                icon: '',
                decs: '',
                path: '/home',
                title: 'Pages.Home.title',
                roleUser: []
            },
            {
                id: 2,
                icon: '',
                decs: '',
                path: '/account/users',
                title: 'Pages.UserManagement.title',
                roleUser: []
            },
            {
                id: 3,
                icon: '',
                decs: '',
                path: '/account/users/Authorization',
                title: 'Pages.UserAuthorization.title',
                roleUser: []
            }
        ];
        return listPages;
    }

    auThorrizationUser(getListPage: any) {
        const usersKey = 'angular-list-role-page-users'
        const listPagesJSON = JSON.stringify(getListPage)
        localStorage.setItem(usersKey, listPagesJSON)
        return getListPage || []
    }

    getRolesList() {
       return this.http.get(`${this.userApiUrl}roles-list`)
       .pipe(map((res: any) => {
            return res.data || [];
        }));
    }

    getMenusList() {
        return this.http.get(`${this.userApiUrl}menus-list`)
        .pipe(map((res: any) => {
            return res.data || [];
        }));
    }

    getUsers(keyword?: string) {
        return this.http.get(`${this.userApiUrl}users-list${keyword ? ('?keyword=' +  keyword) : ''}`)
        .pipe(map((res: any) => {
            return res.data || [];
        }));
    }

    getUserLoginDetail() {
        return this.http.get(`${this.userApiUrl}detail`);
    }

    getUserById(id: string) {        
        return this.http.get(`${this.userApiUrl}user?id=${id}`)
        .pipe(map((res: any) => {
            return res.data;
        }));
    }

    loginUser(username: string, password: string) {
        return this.http.post(`${this.userApiUrl}login?UserName=${username}&Password=${password}`, {})
        .pipe(map((res: any) => {
            const user: any = Object.assign({}, res.userInfo);
            user.token = res.token;
            localStorage.setItem('user', JSON.stringify(user));
            this.userSubject.next(user);
            return user;
        }));
    }
}