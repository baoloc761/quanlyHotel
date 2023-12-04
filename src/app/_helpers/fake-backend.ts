import { Injectable } from '@angular/core';
import { HttpRequest, HttpResponse, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { delay, materialize, dematerialize } from 'rxjs/operators';
import * as UUID from 'uuid'

// array in local storage for registered users
const usersKey = 'angular-tutorial-users';
let users: any[] = JSON.parse(localStorage.getItem(usersKey)!) || [];

@Injectable()
export class FakeBackendInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const { url, method, headers, body } = request;

        return handleRoute();

        function handleRoute() {
            switch (true) {
                case url.endsWith('/users/authenticate') && method === 'POST':
                    return authenticate();
                case url.endsWith('/users/register') && method === 'POST':
                    return register();
                case url.endsWith('/users/edit') && method === 'POST':
                    return updateUser();
                default:
                    // pass through any requests not handled above
                    return next.handle(request);
            }    
        }

        // route functions

        function authenticate() {
            const { username, password } = body;
            const user = users.find(x => x.username === username && x.password === password);
            if (!user) return error('Username or password is incorrect');
            return ok({
                ...basicDetails(user),
                token: 'fake-jwt-token'
            })
        }

        function register() {
            const user = body

            if (users.find(x => x.username === user.username)) {
                return error('Username "' + user.username + '" is already taken')
            }

            user.id = UUID.v4();
            if (user.typeUser.isAdmin) {
                user.claims = [
                    { pageId: 2, canList: true, canEdit: true, canDelete: true, canView: true }
                ]
            } else {
                user.claims = [
                    { pageId: 2, canList: true, canEdit: false, canDelete: false, canView: true }
                ]
            }
            users.push(user);
            localStorage.removeItem(usersKey);
            localStorage.setItem(usersKey, JSON.stringify(users));
            return ok();
        }

        function updateUser() {
            const { user } = body;
            let findUser = users.find(x => x.id === user.id)
            if (!findUser) {
                return error('Username "' + user.username + '" does not existed')
            }
            findUser = {...findUser, ...user}
            const findIndex = users.map((u) => u.id).indexOf(user.id)
            users.splice(findIndex, 1)
            users.push(findUser)
            localStorage.removeItem(usersKey);
            localStorage.setItem(usersKey, JSON.stringify(users));
            return ok();
        }

        // helper functions

        function ok(body?: any) {
            return of(new HttpResponse({ status: 200, body }))
                .pipe(delay(500)); // delay observable to simulate server api call
        }

        function error(message: string) {
            return throwError(() => ({ error: { message } }))
                .pipe(materialize(), delay(500), dematerialize()); // call materialize and dematerialize to ensure delay even if an error is thrown (https://github.com/Reactive-Extensions/RxJS/issues/648);
        }

        function basicDetails(user: any) {
            const { id, username, firstName, lastName, claims } = user;
            return { id, username, firstName, lastName, claims };
        }
    }
}

export const fakeBackendProvider = {
    // use fake backend in place of Http service for backend-less development
    provide: HTTP_INTERCEPTORS,
    useClass: FakeBackendInterceptor,
    multi: true
};