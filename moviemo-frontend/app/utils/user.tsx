import { apiService } from "../services/api";

const usersApiUrl = 'https://localhost:7179/api/users';

export const getCurrentUserId = async () => {
    try {
        const response = await fetch(usersApiUrl + '?username=' + localStorage.getItem('username'), 
        {
            headers: apiService.getHeaders()
        })

        const { id } = await response.json();

        return id;
    } catch (err) {
        console.error('Fetch error: ', err);
    }
}

export const getCurrentUserRole = async () => {
    try {
        const userId = await getCurrentUserId();

        if (userId) {
            const response = await fetch(usersApiUrl + '/' + userId, {
            method: 'GET',
            headers: apiService.getHeaders()
        })

        const data = await response.json();
        return data.userRole;
        }

    } catch (err) {
        console.error('Fetch error: ', err);
    }
}