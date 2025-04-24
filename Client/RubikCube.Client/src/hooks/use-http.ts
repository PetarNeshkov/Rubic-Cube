import axios, { AxiosResponse } from "axios";

const useHttp = {
    get: async <T>(url: string): Promise<T> => {
        const response: AxiosResponse<T> = await axios.get(`${url}`);

        return response.data;
    }, post: async <T, D = unknown>(url: string, data: D): Promise<T> => {
        const response: AxiosResponse<T> = await axios.post(`${url}`, data);

        return response.data;
    }, put: async <T, D = unknown>(url: string, data: D): Promise<T> => {
        const response: AxiosResponse<T> = await axios.put(`${url}`, data);

        return response.data;
    }, delete: async (url: string): Promise<void> => {
        await axios.delete(`${url}`);
    },
};

export default useHttp;