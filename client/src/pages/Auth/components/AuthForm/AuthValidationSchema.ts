import {  z } from 'zod';

export type AuthFormInput = z.infer<typeof AuthValidationSchema>;

export const AuthValidationSchema = z.object({
    userName: z.string().min(2).max(100),
    password: z.string().optional()
})