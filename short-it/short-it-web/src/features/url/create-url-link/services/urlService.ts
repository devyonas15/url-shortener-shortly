import axios from 'axios';
import {
  GenerateUrlRequest,
  GenerateUrlResponse,
} from '../../common/types/UrlDTO';

const CREATE_URL_ENDPOINT = `${import.meta.env.VITE_API_BASE_URL}/url`;

export const createShortUrl = async (
  request: GenerateUrlRequest
): Promise<GenerateUrlResponse> => {
  try {
    const response = await axios.post<GenerateUrlResponse>(
      CREATE_URL_ENDPOINT,
      request
    );

    return response.data;
  } catch (error: any) {
    console.error('Error creating short URL due to:', error);
    throw new Error('Failed to create short URL');
  }
};
