import { Theme } from '@emotion/react';
import { SxProps } from '@mui/material';

export const styles: Record<string, SxProps<Theme>> = {
  pageContainer: {
    mt: 5,
    width: {
      xs: '0.8',
      sm: '0.8',
      md: '0.6',
      lg: '0.3',
      xl: '0.2',
    },
    mx: 'auto',
    my: '15vh',
  },
  pageSubTitle: { mb: 3, color: '#47505F', textAlign: 'center' },
};

export const imageStyle: object = {
  display: 'block',
  margin: '0 auto',
  maxWidth: '60px', // adjust size as needed
  width: '100%',
  height: 'auto',
};
