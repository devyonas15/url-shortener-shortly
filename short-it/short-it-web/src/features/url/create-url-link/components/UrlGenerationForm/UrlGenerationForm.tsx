import { Box, Button, InputLabel, Stack, TextField } from '@mui/material';
import { FC, useState } from 'react';
import { UrlGenerationFormSchema } from '../../schemas/UrlGenerationFormSchema';
import { zodResolver } from '@hookform/resolvers/zod';
import { SubmitHandler, useForm } from 'react-hook-form';
import {
  GenerateUrlRequest,
  GenerateUrlResponse,
} from '../../../common/types/UrlDTO';
import { createShortUrl } from '../../services/urlService';
import { UrlGenerationFormProps } from '../../../common/types/UrlProps';
import { styles } from './UrlGenerationForm.styles';

const UrlGenerationForm: FC<UrlGenerationFormProps> = ({
  onShortUrlGenerated,
  onSuccessfulSubmit,
  onFailedSubmit
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
      onFailedSubmit(true);
      setIsSubmitting(false);
    }
  };

  return (
    <Box
      sx={styles.formContainer}
    >
      <form onSubmit={handleSubmit(onSubmit)}>
        <Stack direction='column' spacing={3}>
          <Box>
            <InputLabel
              htmlFor='url-input'
              sx={styles.urlInputLabel}
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
            sx={styles.generateUrlButton}
          >
            Create your link
          </Button>
        </Stack>
      </form>
    </Box>
  );
};

export default UrlGenerationForm;
