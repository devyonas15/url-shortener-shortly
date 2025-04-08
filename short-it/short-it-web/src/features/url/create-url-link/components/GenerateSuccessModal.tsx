import { Box, Button, Modal, Stack, Typography } from '@mui/material';
import { FC } from 'react';
import { UrlSuccessModalProps } from '../../common/types/UrlProps';
import { CopyAll } from '@mui/icons-material';

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
        sx={{
          backgroundColor: 'white',
          width: '85%',
          margin: 'auto',
          borderRadius: 1,
          p: 3,
          position: 'absolute',
          top: '40%',
          left: '50%',
          transform: 'translate(-50%, -50%)',
        }}
      >
        <Typography
          variant='h3'
          sx={{ fontSize: 22, fontWeight: 'bold', mb: 3 }}
        >
          Your link is ready 🎉
        </Typography>
        <Typography sx={{ fontSize: 13, mb: 5 }}>
          Copy the link below to share it or choose a platform to share it to.
        </Typography>
        <Box sx={{ backgroundColor: 'rgba(210,43,43,.15)', p: 3 }}>
          <Stack
            direction={'row'}
            justifyContent={'space-between'}
            alignItems={'center'}
          >
            <Typography
              sx={{ fontSize: 18, color: '#880808', fontWeight: 'bold' }}
            >
              {shortUrl}
            </Typography>
            <Button
              variant='contained'
              sx={{ backgroundColor: '#D22B2B' }}
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
