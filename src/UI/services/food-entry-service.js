import { httpService } from './http-service';

export const foodEntryService = {
    getMyFoodEntries,
    getFoodEntries,
    getMyCalories,
    addFoodEntry,
    getReport,
    deleteFoodEntry,
    updateFoodEntry
};

const baseUrl = '/users';
async function getMyFoodEntries(page, pageSize, from, to) {
    let url = getUrlWithParams('me/', 'foods', page, pageSize, from, to);
    const result = await httpService.get(url);
    return result;
}

async function getFoodEntries(page, pageSize, from, to) {
    let url = getUrlWithParams('', 'foods', page, pageSize, from, to);
    const result = await httpService.get(url);
    return result;
}

async function getMyCalories(page, pageSize, from, to) {
    let url = getUrlWithParams('me/', 'calories', page, pageSize, from, to);
    const result = await httpService.get(url);
    return result;
}

async function addFoodEntry(userId, foodEntry) {
    const result = await httpService.post(`${baseUrl}/${userId}/foods`, foodEntry);
    return result;
}

async function updateFoodEntry(userId, foodEntryId, foodEntry) {
    const result = await httpService.put(`${baseUrl}/${userId}/foods/${foodEntryId}`, foodEntry);
    return result;
}

async function deleteFoodEntry(userId, foodEntryId) {
    const result = await httpService.delete(`${baseUrl}/${userId}/foods/${foodEntryId}`);
    return result;
}

async function getReport(page, pageSize, to) {
    const result = await httpService.get(`${baseUrl}/foods/report?page=${page}&pageSize=${pageSize}&to=${to}`);
    return result;
}

function getUrlWithParams(userId, resource, page, pageSize, from, to) {
    let url = `${baseUrl}/${userId}${resource}?page=${page}&pageSize=${pageSize}&`;
    if (from) {
        url += `from=${from}&`;
    }
    if (to) {
        url += `to=${to}`;
    }
    return url;
}