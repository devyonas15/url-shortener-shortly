import { Theme } from '@emotion/react';
import { SxProps } from '@mui/material';

export const styles: Record<string, SxProps<Theme>> = {
  pageContainer: { mt: 5, 
    width: {
      xs: '0.95',
      sm: '0.9',
      md: '0.8',
      lg: '0.7',
      xl: '0.6'
    },
    mx: 'auto'
   },
  pageTitle: {
    mb: 3,
    color: '#47505F',
    fontWeight: 'bold',
    fontSize: 30,
  },
  pageSubTitle: {mb: 3, color: '#47505F' },
};
