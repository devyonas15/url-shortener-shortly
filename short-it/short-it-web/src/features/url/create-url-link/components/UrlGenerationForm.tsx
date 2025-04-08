import { Box, Button, InputLabel, Stack, TextField } from '@mui/material';
import { FC, useState } from 'react';
import { UrlGenerationFormSchema } from '../schemas/UrlGenerationFormSchema';
import { zodResolver } from '@hookform/resolvers/zod';
import { SubmitHandler, useForm } from 'react-hook-form';
import {
  GenerateUrlRequest,
  GenerateUrlResponse,
} from '../../common/types/UrlDTO';
import { createShortUrl } from '../services/urlService';
import { UrlGenerationFormProps } from '../../common/types/UrlProps';

const UrlGenerationForm: FC<UrlGenerationFormProps> = ({
  onShortUrlGenerated,
  onSuccessfulSubmit,
}) => {
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const {
    register,
    handleSubmit,
    formState: { errors },
    getValues,
  } = useForm<GenerateUrlRequest>({
    mode: 'onChange',
    reValidateMode: 'onChange',
    resolver: zodResolver(UrlGenerationFormSchema),
    defaultValues: {
      longUrl: '',
    },
  });

  const onSubmit: SubmitHandler<GenerateUrlRequest> = async (
    data: GenerateUrlRequest
  ) => {
    setIsSubmitting(true);

    try {
      const result: GenerateUrlResponse = await createShortUrl(data);
      onShortUrlGenerated(result.shortUrl);
      onSuccessfulSubmit(true);
      setIsSubmitting(false);
    } catch (error: any) {
      console.error(error.message);
      setIsSubmitting(false);
    }
  };

  return (
    <Box
      sx={{
        width: 0.9,
        margin: 'auto',
        backgroundColor: 'white',
        padding: 2,
        borderRadius: '10px',
      }}
    >
      <form onSubmit={handleSubmit(onSubmit)}>
        <Stack direction='column' spacing={3}>
          <Box>
            <InputLabel
              htmlFor='url-input'
              sx={{ mb: 1.5, fontWeight: 'bold', color: 'black' }}
            >
              Destination
            </InputLabel>
            <TextField
              {...register('longUrl')}
              fullWidth
              id='url-input'
              className='url-form__input'
              placeholder='https://my-example-url.com'
              slotProps={{
                inputLabel: {
                  shrink: false,
                },
              }}
              error={!!errors.longUrl}
              helperText={errors.longUrl?.message}
            />
          </Box>
          <Button
            loading={isSubmitting}
            disabled={!!errors.longUrl || getValues('longUrl') === ''}
            type='submit'
            variant='contained'
            sx={{ mt: 5, height: 50, backgroundColor: '#D22B2B' }}
          >
            Create your link
          </Button>
        </Stack>
      </form>
    </Box>
  );
};

export default UrlGenerationForm;
