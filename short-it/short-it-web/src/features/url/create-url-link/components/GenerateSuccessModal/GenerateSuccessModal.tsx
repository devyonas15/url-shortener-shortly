import { Box, Button, Modal, Stack, Typography } from '@mui/material';
import { FC } from 'react';
import { UrlSuccessModalProps } from '../../../common/types/UrlProps';
import { CopyAll } from '@mui/icons-material';
import { styles } from './GenerateSuccessModal.styles';

const GenerateSuccessModal: FC<UrlSuccessModalProps> = ({
  shortUrl,
  isOpen,
  onCopyToClipboard,
  onClose,
}) => {
  const handleCopyToClipboard = async (shortUrl: string | undefined) => {
    try {
      if (shortUrl) {
        await navigator.clipboard.writeText(shortUrl);
        onCopyToClipboard(true);
      }
    } catch (error: any) {
      console.error('Failed to copy short URL to clipboard:', error);
    }
  };

  return (
    <Modal open={isOpen} onClose={onClose}>
      <Box
        sx={styles.modalBox}
      >
        <Typography
          variant='h3'
          sx={styles.modalTitle}
        >
          Your link is ready 🎉
        </Typography>
        <Typography sx={styles.modalSubTitle}>
          Copy the link below to share it or choose a platform to share it to.
        </Typography>
        <Box sx={styles.shortUrlBox}>
          <Stack
            direction={'row'}
            justifyContent={'space-between'}
            alignItems={'center'}
          >
            <Typography
              sx={styles.shortUrlLink}
            >
              {shortUrl}
            </Typography>
            <Button
              variant='contained'
              sx={styles.copyButton}
              startIcon={<CopyAll />}
              onClick={() => handleCopyToClipboard(shortUrl)}
            >
              copy
            </Button>
          </Stack>
        </Box>
      </Box>
    </Modal>
  );
};

export default GenerateSuccessModal;
