import { Box, Typography } from '@mui/material';
import { FC, useEffect, useState } from 'react';
import UrlGenerationForm from '../components/UrlGenerationForm';
import GenerateSuccessModal from '../components/GenerateSuccessModal';
import NotificationBar from '../../../shared/components/Notification';
import CheckIcon from '@mui/icons-material/Check';

const CreateLinkPage: FC = () => {
  const [shortUrl, setShortUrl] = useState<string | undefined>('');
  const [isSuccessModalOpen, setIsSuccessModalOpen] = useState<boolean>(false);
  const [isNotificationOpen, setIsNotificationOpen] = useState<boolean>(false);

  // Manual close of the NotificationBar after 1 second
  useEffect(() => {
    if (isNotificationOpen) {
      const timer = setTimeout(() => {
        setIsNotificationOpen(false);
      }, 1000);

      return () => clearTimeout(timer);
    }
  }, [isNotificationOpen]);

  return (
    <Box sx={{ mt: 5 }}>
      <Typography
        sx={{
          mb: 3,
          ml: 1,
          color: '#47505F',
          fontWeight: 'bold',
          fontSize: 30,
        }}
      >
        Create a link
      </Typography>
      <Typography sx={{ ml: 1, mb: 3, color: '#47505F' }}>
        You can create links by filling up the form.
      </Typography>
      <UrlGenerationForm
        onShortUrlGenerated={setShortUrl}
        onSuccessfulSubmit={setIsSuccessModalOpen}
      />
      <GenerateSuccessModal
        onClose={() => setIsSuccessModalOpen(false)}
        isOpen={isSuccessModalOpen}
        shortUrl={shortUrl}
        onCopyToClipboard={setIsNotificationOpen}
      />
      {isNotificationOpen && (
        <NotificationBar
          message='URL is successfully copied to clipboard'
          severity='success'
          icon={<CheckIcon fontSize='inherit' />}
        />
      )}
    </Box>
  );
};

export default CreateLinkPage;
