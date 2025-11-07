
import { useCallback, useEffect, useState } from "react";
import { Box, Button, Input, Field } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { changePassword } from "../../../../api/auth/authApi";
import { ChangePasswordFormInput, ChangePasswordValidationSchema } from "./ChangePasswordFormValidationSchema";
import { Nullable } from "../../../../shared/utilities/nullable";


interface Props {
    defaultPasswordResetValues: { userName: string, currentPassword: Nullable<string> };
    onTokenReceived: (token: string) => void;
}

const ChangePasswordForm: React.FC<Props> = ({defaultPasswordResetValues, onTokenReceived}) => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const getDefaultValues = useCallback(() => {
        return {
            userName: defaultPasswordResetValues?.userName ?? "",
            currentPassword: defaultPasswordResetValues?.currentPassword ?? "",
            newPassword: ""
        }
    }, [defaultPasswordResetValues])

    const { register, handleSubmit, formState: { errors }, reset} = useForm<ChangePasswordFormInput>({
        resolver: zodResolver(ChangePasswordValidationSchema),
        mode: "onBlur",
        defaultValues: getDefaultValues()
    });

    useEffect(() => {
        reset(getDefaultValues());
    }, [defaultPasswordResetValues, reset, getDefaultValues])


    const onSubmit = async (authData: ChangePasswordFormInput) => {
        setError("");
        try {
            const token = await changePassword(authData.userName, authData.currentPassword,
                authData.newPassword
            );

            if (token) {
                onTokenReceived(token);
                return;
            } else {
                setError(t("change_password_form_error"));
            }
        } catch (err: unknown) {
            if (err instanceof Error) {
                setError(err.message || t("change_password_form_error"));
            } else {
                setError(t("change_password_form_error"));
            }
        } finally {
            setLoading(false);
        }
    }

    const {t} = useTranslation();

    return (
        <Box as="form" onSubmit={handleSubmit(onSubmit)}>
            <h2 className="auth-title" style={{ textAlign: "center", marginBottom: "2rem", fontWeight: 700, fontSize: "2rem" }}>
                {t("change_password_form_title")}
            </h2>
            <Field.Root invalid={!!errors.userName}>
                <Field.Label>{t("change_password_form_username")}</Field.Label>
                <Input {...register("userName")} autoComplete="off" />
                <Field.ErrorText>{errors.userName?.message}</Field.ErrorText>
            </Field.Root>
            <Field.Root invalid={!!errors.currentPassword}>
                <Field.Label>{t("change_password_form_current_password")}</Field.Label>
                <Input type="password" {...register("currentPassword")} autoComplete="off" />
                <Field.ErrorText>{errors.currentPassword?.message}</Field.ErrorText>
            </Field.Root>
            <Field.Root invalid={!!errors.newPassword}>
                <Field.Label>{t("change_password_form_new_password")}</Field.Label>
                <Input type="password" {...register("newPassword")} autoComplete="off" />
                <Field.ErrorText>{errors.newPassword?.message}</Field.ErrorText>
            </Field.Root>
            {error && <div className="auth-error" style={{ color: "#e53e3e", margin: "1rem 0", textAlign: "center" }}>{error}</div>}
            <Button loading={loading} type="submit" colorScheme="teal" w="full" mt={4} size="lg">{t("change_password_form_submit_button")}</Button>
        </Box>
    
    );
};

export default ChangePasswordForm;
