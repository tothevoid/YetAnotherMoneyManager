import {  z } from 'zod';

export type ChangePasswordFormInput = z.infer<typeof ChangePasswordValidationSchema>;

export const ChangePasswordValidationSchema = z.object({
    userName: z.string().min(2).max(100),
    currentPassword: z.string().optional(),
    newPassword: z.string().min(2).max(100),
})