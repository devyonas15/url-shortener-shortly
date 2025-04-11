import { createTheme, Theme } from '@mui/material';

const hoverColor = '#D22B2B';
const defaultTextColor = '#555555';

export const dashboardTheme: Theme = createTheme({
  components: {
    MuiListItemButton: {
      styleOverrides: {
        root: () => {
          return {
            marginBottom: '15px',
            '&:hover': {
              backgroundColor: `${hoverColor}10`,
            },
            '&.Mui-selected': {
              backgroundColor: `${hoverColor}10`,
              // Apply red text for the selected item
              '& .MuiListItemText-root': {
                '& .MuiTypography-root, & span': {
                  color: `${hoverColor} !important`,
                  fontWeight: 600,
                },
              },
              '&:hover': {
                backgroundColor: `${hoverColor}10`,
              },
            },
          };
        },
      },
    },
    MuiListItemIcon: {
      styleOverrides: {
        root: () => {
          return {
            color: `${hoverColor} !important`,
            transition: 'color 0.2s ease',
            '& .MuiSvgIcon-root': {
              fill: `${hoverColor} !important`,
              color: `${hoverColor} !important`,
            },
          };
        },
      },
    },
    MuiListItemText: {
      styleOverrides: {
        root: () => {
          return {
            '& .MuiTypography-root': {
              color: defaultTextColor,
              fontWeight: 600,
              fontSize: '0.95rem',
            },
            '& span': {
              color: defaultTextColor,
              fontWeight: 600,
            },
          };
        },
      },
    },
    MuiSvgIcon: {
      styleOverrides: {
        root: {
          fill: `${hoverColor} !important`,
          color: `${hoverColor} !important`,
        },
      },
    },
  },
  colorSchemes: { light: true },
  cssVariables: {
    colorSchemeSelector: 'class',
  },
  breakpoints: {
    values: {
      xs: 0,
      sm: 600,
      md: 600,
      lg: 1200,
      xl: 1536,
    },
  },
});
