import { zodResolver } from '@hookform/resolvers/zod';
import { LoginFormSchema } from '../schemas/LoginFormSchema';
import { LoginRequest, LoginResponse } from '../types/LoginDTO';
import { FC, useState } from 'react';
import { SubmitHandler, useForm } from 'react-hook-form';
import { login } from '../services/loginService';
import { Box, Button, InputLabel, Stack, TextField } from '@mui/material';
import { styles } from './LoginForm.styles';
import { LoginProps } from '../types/LoginProps';
import { setSessionItem } from '../../../shared/utils/storageUtils/sessionStorageUtils';
import { SESSION_DATA } from '../../../shared/utils/constants/SessionStorageKey';
import { useNavigate } from 'react-router-dom';

const LoginForm: FC<LoginProps> = ({ onSuccessfulLoginCheck }) => {
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors },
    getValues,
  } = useForm<LoginRequest>({
    mode: 'onChange',
    reValidateMode: 'onChange',
    resolver: zodResolver(LoginFormSchema),
    defaultValues: {
      email: '',
      password: '',
    },
  });

  const onSubmit: SubmitHandler<LoginRequest> = async (data: LoginRequest) => {
    setIsSubmitting(true);

    try {
      const result: LoginResponse = await login(data);

      setSessionItem(SESSION_DATA, result, true);
      setIsSubmitting(false);
      onSuccessfulLoginCheck(true);

      navigate('/');
    } catch (error: any) {
      console.error(error.message);
      setIsSubmitting(false);
      onSuccessfulLoginCheck(false);
    }
  };

  return (
    <Box sx={styles.formContainer}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Stack direction='column' spacing={3}>
          <Box>
            <InputLabel htmlFor='email-input' sx={styles.inputLabel}>
              Email
            </InputLabel>
            <TextField
              {...register('email')}
              fullWidth
              id='url-input'
              className='login-form__input'
              placeholder='e.g. short@mailinator.com'
              slotProps={{
                inputLabel: {
                  shrink: false,
                },
              }}
              error={!!errors.email}
              helperText={errors.email?.message}
            />
          </Box>
          <Box>
            <InputLabel htmlFor='password-input' sx={styles.inputLabel}>
              Password
            </InputLabel>
            <TextField
              {...register('password')}
              fullWidth
              id='password-input'
              className='login-form__input'
              placeholder='******'
              slotProps={{
                inputLabel: {
                  shrink: false,
                },
              }}
              error={!!errors.password}
              helperText={errors.password?.message}
            />
          </Box>
          <Button
            loading={isSubmitting}
            disabled={
              !!errors.email ||
              getValues('email') === '' ||
              !!errors.password ||
              getValues('password') === ''
            }
            type='submit'
            variant='contained'
            sx={styles.loginButton}
          >
            Login
          </Button>
        </Stack>
      </form>
    </Box>
  );
};

export default LoginForm;
