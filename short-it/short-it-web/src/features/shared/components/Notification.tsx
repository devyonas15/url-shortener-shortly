import { Alert, AlertTitle, Snackbar } from '@mui/material';
import { FC } from 'react';
import { NotificationProps } from '../types/NotificationProps';

const NotificationBar: FC<NotificationProps> = ({
  title,
  message,
  severity,
  icon,
}) => {
  return (
    <Snackbar
      open={true}
      anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
      sx={{ mb: 2 }}
    >
      <Alert icon={icon} severity={severity}>
        {title && <AlertTitle>{title}</AlertTitle>}
        {message}
      </Alert>
    </Snackbar>
  );
};

export default NotificationBar;
