import axios from 'axios';
import { LoginRequest, LoginResponse } from '../types/LoginDTO';

const LOGIN_ENDPOINT = `${import.meta.env.VITE_API_BASE_URL}/auth/login`;

export const login = async (request: LoginRequest): Promise<LoginResponse> => {
  try {
    const response = await axios.post<LoginResponse>(LOGIN_ENDPOINT, request);

    return response.data;
  } catch (error: any) {
    console.error('Error while login due to:', error);
    throw new Error('Error while login, please try again later.');
  }
};
