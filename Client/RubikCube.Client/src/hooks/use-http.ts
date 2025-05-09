﻿import axios, { AxiosResponse } from "axios";

const useHttp = {
    get: async <T>(url: string): Promise<T> => {
        const response: AxiosResponse<T> = await axios.get(`${url}`);

        return response.data;
    },post: async <T>(url: string, data: unknown): Promise<T> => {
        const response: AxiosResponse<T> = await axios.post(`${url}`, data, {
            headers: {
                'Content-Type': 'application/json',
            }
        });
        return response.data;
    }, put: async <T>(url: string, data: unknown): Promise<T> => {
        const response: AxiosResponse<T> = await axios.put(`${url}`, data, {
            headers: {
                'Content-Type': 'application/json',
            }
        });
        
        return response.data;
    }, delete: async (url: string): Promise<void> => {
        await axios.delete(`${url}`);
    },
};

export default useHttp;