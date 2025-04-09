import { Box, Typography } from '@mui/material';
import { FC, useEffect, useState } from 'react';
import GenerateSuccessModal from '../components/GenerateSuccessModal/GenerateSuccessModal';
import NotificationBar from '../../../shared/components/Notification';
import CheckIcon from '@mui/icons-material/Check';
import CancelIcon from '@mui/icons-material/Cancel';
import UrlGenerationForm from '../components/UrlGenerationForm/UrlGenerationForm';
import { styles } from './CreateLinkPage.styles';

const CreateLinkPage: FC = () => {
  const [shortUrl, setShortUrl] = useState<string | undefined>('');
  const [isSuccessModalOpen, setIsSuccessModalOpen] = useState<boolean>(false);
  const [isSuccessNotificationOpen, setisSuccessNotificationOpen] = useState<boolean>(false);
  const [isFailedNotificationOpen, setIsFailedNotificationOpen] = useState<boolean>(false);

  // Manual close of the NotificationBar after 1 second
  useEffect(() => {
    if (isSuccessNotificationOpen || isFailedNotificationOpen) {
      const timer = setTimeout(() => {
        isSuccessNotificationOpen ? setisSuccessNotificationOpen(false) : setIsFailedNotificationOpen(false);
      }, 1000);

      return () => clearTimeout(timer);
    }
  }, [isSuccessNotificationOpen, isFailedNotificationOpen]);

  return (
    <Box sx={styles.pageContainer}>
      <Typography
        sx={styles.pageTitle}
      >
        Create a link
      </Typography>
      <Typography sx={styles.pageSubTitle}>
        You can create links by filling up the form.
      </Typography>
      <UrlGenerationForm
        onShortUrlGenerated={setShortUrl}
        onSuccessfulSubmit={setIsSuccessModalOpen}
        onFailedSubmit={setIsFailedNotificationOpen}
      />
      <GenerateSuccessModal
        onClose={() => setIsSuccessModalOpen(false)}
        isOpen={isSuccessModalOpen}
        shortUrl={shortUrl}
        onCopyToClipboard={setisSuccessNotificationOpen}
      />
      {isSuccessNotificationOpen && (
        <NotificationBar
          message='URL is successfully copied to clipboard'
          severity='success'
          icon={<CheckIcon fontSize='inherit' />}
        />
      )}
      {isFailedNotificationOpen && (
        <NotificationBar
          message='Failed to generate URL'
          severity='error'
          icon={<CancelIcon fontSize='inherit' />}
        />
        )}
    </Box>
  );
};

export default CreateLinkPage;
