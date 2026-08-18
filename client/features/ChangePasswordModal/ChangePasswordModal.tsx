import React, { forwardRef, useImperativeHandle, useMemo, useRef, useState } from 'react';
import { Field, Input, Stack, Text } from '@chakra-ui/react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslation } from 'react-i18next';
import { BaseModalRef } from '../../src/shared/utilities/modalUtilities';
import BaseFormModal from '../../src/shared/modals/BaseFormModal/BaseFormModal';
import { useUserProfile } from '../UserProfileSettingsModal/hooks/UserProfileContext';
import { changePassword } from '../../src/api/auth/authApi';
import {
    ChangePasswordModalInput,
    getChangePasswordModalValidationSchema
} from './ChangePasswordModalValidationSchema';

const ChangePasswordModal = forwardRef<BaseModalRef>((_, ref) => {
    const { t } = useTranslation();
    const { user } = useUserProfile();
    const modalRef = useRef<BaseModalRef>(null);
    const [apiError, setApiError] = useState<string | null>(null);

    const validationSchema = useMemo(() => getChangePasswordModalValidationSchema(t), [t]);

    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
        reset
    } = useForm<ChangePasswordModalInput>({
        resolver: zodResolver(validationSchema),
        mode: 'onBlur',
        defaultValues: {
            currentPassword: '',
            newPassword: '',
            confirmPassword: ''
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: () => {
            setApiError(null);
            reset();
            modalRef.current?.openModal();
        },
        closeModal: () => {
            setApiError(null);
            reset();
            modalRef.current?.closeModal();
        }
    }));

    const onSubmit = async (data: ChangePasswordModalInput) => {
        setApiError(null);
        if (!user?.userName) return;

        try {
            const token = await changePassword(user.userName, data.currentPassword ?? '', data.newPassword);
            if (token) {
                modalRef.current?.closeModal();
                reset();
            } else {
                setApiError(t('change_password_form_error'));
            }
        } catch {
            setApiError(t('change_password_form_error'));
        }
    };

    return (
        <BaseFormModal
            ref={modalRef}
            title={t('change_password_form_title')}
            submitHandler={handleSubmit(onSubmit)}
            saveButtonTitle={isSubmitting ? '...' : t('change_password_form_submit_button')}
        >
            <Stack gap={4}>
                {apiError && (
                    <Text color="red.400" fontSize="sm">
                        {apiError}
                    </Text>
                )}

                <Field.Root invalid={!!errors.currentPassword}>
                    <Field.Label color="text_primary">{t('change_password_form_current_password')}</Field.Label>
                    <Input
                        type="password"
                        {...register('currentPassword')}
                        backgroundColor="background_primary"
                        borderColor="border_primary"
                        color="text_primary"
                    />
                    <Field.ErrorText>{errors.currentPassword?.message}</Field.ErrorText>
                </Field.Root>

                <Field.Root invalid={!!errors.newPassword}>
                    <Field.Label color="text_primary">{t('change_password_form_new_password')}</Field.Label>
                    <Input
                        type="password"
                        {...register('newPassword')}
                        backgroundColor="background_primary"
                        borderColor="border_primary"
                        color="text_primary"
                    />
                    <Field.ErrorText>{errors.newPassword?.message}</Field.ErrorText>
                </Field.Root>

                <Field.Root invalid={!!errors.confirmPassword}>
                    <Field.Label color="text_primary">{t('change_password_form_confirm_password')}</Field.Label>
                    <Input
                        type="password"
                        {...register('confirmPassword')}
                        backgroundColor="background_primary"
                        borderColor="border_primary"
                        color="text_primary"
                    />
                    <Field.ErrorText>{errors.confirmPassword?.message}</Field.ErrorText>
                </Field.Root>
            </Stack>
        </BaseFormModal>
    );
});

ChangePasswordModal.displayName = 'ChangePasswordModal';

export default ChangePasswordModal;
