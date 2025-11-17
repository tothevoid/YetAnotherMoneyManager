
import { useState } from "react";
import { Box, Button, Input, Field } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { auth } from "../../../../api/auth/authApi";
import { AuthFormInput, AuthValidationSchema } from "./AuthValidationSchema";
import { Nullable } from "../../../../shared/utilities/nullable";

interface Props {
    onPasswordChangeRequired: (userName: string, currentPassword: Nullable<string>) => void;
    onTokenReceived: (token: string) => void;
}

const AuthPage: React.FC<Props> = ({ onPasswordChangeRequired, onTokenReceived }) => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
   
    const { register, handleSubmit, watch, formState: { errors }} = useForm<AuthFormInput>({
        resolver: zodResolver(AuthValidationSchema),
        mode: "onBlur",
        defaultValues: {
            userName: "",
            password: ""
        }
    });

    const userName = watch("userName");
    const password = watch("password");

    const onSubmit = async (authData: AuthFormInput) => {
        setError("");
        try {
            const authInfo = await auth(authData.userName, authData.password);

            if (authInfo) {
                
                if (authInfo.passwordChangeRequired) {
                    onPasswordChangeRequired(userName, password);
                    return;
                }

                if (authInfo.token) {
                    onTokenReceived(authInfo.token);
                }
            } else {
                setError(t("auth_form_invalid_credentials"));
            }
        } catch (err: unknown) {
            if (err instanceof Error) {
                setError(err.message || t("auth_form_error"));
            } else {
                setError(t("auth_form_error"));
            }
        } finally {
            setLoading(false);
        }
    }

    const {t} = useTranslation();

    return (
       <Box as="form" onSubmit={handleSubmit(onSubmit)}>
            <h2 className="auth-title" style={{ textAlign: "center", marginBottom: "2rem", fontWeight: 700, fontSize: "2rem" }}>
                {t("auth_form_title")}
            </h2>
            <Field.Root invalid={!!errors.userName}>
                <Field.Label>{t("auth_form_username")}</Field.Label>
                <Input {...register("userName")} />
                <Field.ErrorText>{errors.userName?.message}</Field.ErrorText>
            </Field.Root>
            <Field.Root invalid={!!errors.password}>
                <Field.Label>{t("auth_form_password")}</Field.Label>
                <Input type="password" {...register("password")} />
                <Field.ErrorText>{errors.password?.message}</Field.ErrorText>
            </Field.Root>
            {error && <div className="auth-error" style={{ color: "#e53e3e", margin: "1rem 0", textAlign: "center" }}>{error}</div>}
            <Button loading={loading} type="submit" colorScheme="teal" w="full" mt={4} size="lg">{t("auth_form_login")}</Button>
        </Box>
    );
};

export default AuthPage;
