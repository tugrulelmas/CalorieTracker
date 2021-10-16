import getConfig from 'next/config';
import { userService } from './user-service';

const { publicRuntimeConfig } = getConfig();
const baseUrl = publicRuntimeConfig.apiUrl;

export const httpService = {
    get,
    post,
    put,
    delete: _delete
};

async function get(url) {
    const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json', ...getAuthHeader() }
    };
    const response = await fetch(`${baseUrl}${url}`, requestOptions);
    return handleResponse(response);
}

async function post(url, body) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', ...getAuthHeader() },
        body: JSON.stringify(body)
    };
    const response = await fetch(`${baseUrl}${url}`, requestOptions);

    return handleResponse(response);
}

async function put(url, body) {
    const requestOptions = {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', ...getAuthHeader() },
        body: JSON.stringify(body)
    };
    const response = await fetch(`${baseUrl}${url}`, requestOptions);

    return handleResponse(response);
}

async function _delete(url) {
    const requestOptions = {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', ...getAuthHeader() }
    };
    const response = await fetch(`${baseUrl}${url}`, requestOptions);

    return handleResponse(response);
}

async function handleResponse(response) {
    if (response.status === 401 || response.status === 403) {
        userService.logout();
        return;
    }

    const result = await response.json().catch(e => {
        console.log("in handle", e);
        return "";
    });
    if (response.status < 200 || response.status >= 400) {
        if (result.errorMessage) {
            return Promise.reject(result.errorMessage);
        }

        return Promise.reject("Unexpected error occured.");
    }

    return Promise.resolve(result);
}

function getAuthHeader() {
    const user = userService.userValue;
    if (!user || !user.token)
        return {};

    return { Authorization: `Bearer ${user.token}` };
}