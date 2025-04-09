import { Theme } from "@emotion/react";
import { SxProps } from "@mui/material";

export const styles: Record<string, SxProps<Theme>> = {
  modalBox: {
    backgroundColor: 'white',
    width: {
      xs: '0.8',
      sm: '0.7',
      md: '0.5',
      lg: '0.4',
      xl: '0.2'
    },
    margin: 'auto',
    borderRadius: 1,
    p: 3,
    position: 'absolute',
    top: '40%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
  },
  modalTitle: { fontSize: 22, fontWeight: 'bold', mb: 3 },
  modalSubTitle: { fontSize: 13, mb: 5 },
  shortUrlBox: { fontSize: 13, mb: 5 },
  shortUrlLink: { fontSize: 18, color: '#880808', fontWeight: 'bold' },
  copyButton: { backgroundColor: '#D22B2B' },
};
