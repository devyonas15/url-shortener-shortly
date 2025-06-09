import z, { ZodType } from 'zod';
import { LoginRequest } from '../types/LoginDTO';

const LoginFormSchema: ZodType<LoginRequest> = z.object({
  email: z.string().email('Please enter a valid email.'),
  password: z
    .string()
    .min(8)
    .regex(
      /^(?=.*[A-Za-z])(?=.*\d)(?=.*[\W_]).+$/,
      'Password must consist of alphanumeric and special characters.'
    ),
});

export { LoginFormSchema };
