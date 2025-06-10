import { Box, Typography } from '@mui/material';
import LoginForm from '../components/LoginForm';
import CheckIcon from '@mui/icons-material/Check';
import CancelIcon from '@mui/icons-material/Cancel';
import { imageStyle, styles } from './LoginPage.styles';
import { useEffect, useState } from 'react';
import NotificationBar from '../../../shared/components/Notification';

const LoginPage = () => {
  const [isLoginSucceed, setIsLoginSucceed] = useState<boolean | null>(null);

  useEffect(() => {
    if (isLoginSucceed !== null) {
      const timer = setTimeout(() => {
        setIsLoginSucceed(null); // Clear the notification after 1 second
      }, 1000);

      return () => clearTimeout(timer);
    }
  }, [isLoginSucceed]);

  return (
    <Box sx={styles.pageContainer}>
      <img
        style={imageStyle}
        src='../../../public/short-it.svg'
        alt='Short It icon'
      />
      <Typography variant='body1' sx={styles.pageSubTitle}>
        Hi! we are glad to see you again.
      </Typography>
      <LoginForm onSuccessfulLoginCheck={setIsLoginSucceed} />
      {isLoginSucceed !== null && isLoginSucceed && (
        <NotificationBar
          message='Successfully login to the system'
          severity='success'
          icon={<CheckIcon fontSize='inherit' />}
        />
      )}
      {isLoginSucceed !== null && !isLoginSucceed && (
        <NotificationBar
          message='Failed to login to the system. Please try again later'
          severity='error'
          icon={<CancelIcon fontSize='inherit' />}
        />
      )}
    </Box>
  );
};

export default LoginPage;
