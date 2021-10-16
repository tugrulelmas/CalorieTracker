import { BehaviorSubject } from 'rxjs';
import { httpService } from './http-service';
import Router from 'next/router';


const userSubject = new BehaviorSubject(process.browser && JSON.parse(localStorage.getItem('user')));

const baseUrl = '/users';
export const userService = {
    user: userSubject.asObservable(),
    get userValue() { return userSubject.value },
    login,
    logout,
    isAdmin,
    getUsers
};

function isAdmin() {
    if (!userSubject.value)
        return false;

    return userSubject.value.roles.includes('admin')
}

async function login(email) {
    const user = await httpService.post('/tokens', { email: email });
    userSubject.next(user);
    localStorage.setItem('user', JSON.stringify(user));
    return user;
}

function logout() {
    localStorage.removeItem('user');
    userSubject.next(null);
    Router.push('/account/login');
}

async function getUsers() {
    const result = await httpService.get(baseUrl);
    return result;
}