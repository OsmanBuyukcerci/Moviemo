import { apiService } from "../services/api";


export const getCurrentUserId = async () => {
    const usersApiUrl = 'https://localhost:7179/api/users';

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