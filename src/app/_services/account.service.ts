import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '@environments/environment';
import { User } from '@app/_models';

// array in local storage for registered users

@Injectable({ providedIn: 'root' })
export class AccountService {
    private userSubject: BehaviorSubject<User | null>;
    public user: Observable<User | null>;

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

    getUsers(keyword?: string) {        
        const usersKey = 'angular-tutorial-users';
        const users: any[] = JSON.parse(localStorage.getItem(usersKey)!) || [];
        if (_.isEmpty(keyword)) return Object.assign([], users);
        return _.filter(users, (u) => u.username.toLowerCase().indexOf(keyword?.toLocaleLowerCase()) !== -1 ||
        u.firstName.toLowerCase().indexOf(keyword?.toLocaleLowerCase()) !== -1 ||
        u.lastName.toLowerCase().indexOf(keyword?.toLocaleLowerCase()) !== -1)
    }

    getUserById(id?: string) {        
        const usersKey = 'angular-tutorial-users';
        const users: any[] = JSON.parse(localStorage.getItem(usersKey)!) || [];
        if (_.isEmpty(id)) return Object.assign({}, users[0]);
        return _.find(users, (u) => u.id === id)
    }
}