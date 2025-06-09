import { Theme } from '@emotion/react';
import { SxProps } from '@mui/material';

export const styles: Record<string, SxProps<Theme>> = {
  formContainer: {
    backgroundColor: 'white',
    padding: 2,
    borderRadius: '10px',
  },
  inputLabel: { mb: 1.5, fontWeight: 'bold', color: 'black' },
  loginButton: { mt: 5, height: 50, backgroundColor: '#D22B2B' },
};
