import { z } from 'zod';
import { TFunction } from 'i18next';

export const getChangePasswordValidationSchema = (t: TFunction) => z.object({
    userName: z.string().min(1, t("validation_login_required")).max(100),
    currentPassword: z.string().optional(),
    newPassword: z.string().min(6, t("validation_password_min_length")).max(100),
});

export type ChangePasswordFormInput = z.infer<ReturnType<typeof getChangePasswordValidationSchema>>;