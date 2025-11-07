
import { useState } from "react";
import { Box, Button, Input, Field, Flex } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import { AuthFormInput, AuthValidationSchema } from "./AuthValidationSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import config from "../../config";
import "./AuthPage.scss";
import { useTranslation } from "react-i18next";
import { useLocation, useNavigate } from "react-router-dom";

const AuthPage: React.FC = () => {

	const [loading, setLoading] = useState(false);
	const [error, setError] = useState("");

    const { register, handleSubmit, formState: { errors }} = useForm<AuthFormInput>({
        resolver: zodResolver(AuthValidationSchema),
        mode: "onBlur",
        defaultValues: {
            userName: "",
            password: ""
        }
    });

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from ?? "/";

    const onSubmit = async (auth: AuthFormInput) => {
        setError("");
        try {
            const basicUrl = `${config.api.URL}/Auth/Login`;
            const response = await fetch(basicUrl, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ userName: auth.userName, password: auth.password ?? null })
            });
            if (!response.ok) throw new Error(t("auth_page_error_invalid_credentials"));
            const data = await response.json();

            if (data.token) {
                localStorage.setItem("auth_token", data.token);
                navigate(from, { replace: true });
            }
            
        } catch (err: unknown) {
            if (err instanceof Error) {
                setError(err.message || t("auth_page_error"));
            } else {
                setError(t("auth_page_error"));
            }
        } finally {
            setLoading(false);
        }
    }

    const {t} = useTranslation();

    return (
        <Flex minH="100vh" w="100vw" align="center" justify="center" 
            bgGradient="linear(120deg, #e0eafc 0%, #cfdef3 100%)">
            <Box as="form" onSubmit={handleSubmit(onSubmit)}
                maxW="sm"
                w="100%"
                p={8}
                borderRadius="xl"
                boxShadow="lg"
                bg="white"
            >
                <h2 className="auth-title" style={{ textAlign: "center", marginBottom: "2rem", fontWeight: 700, fontSize: "2rem" }}>
                    {t("auth_page_title")}
                </h2>
                <Field.Root invalid={!!errors.userName}>
                    <Field.Label>{t("auth_page_username")}</Field.Label>
                    <Input {...register("userName")} autoComplete="off" placeholder={t("auth_page_username")} />
                    <Field.ErrorText>{errors.userName?.message}</Field.ErrorText>
                </Field.Root>
                <Field.Root invalid={!!errors.password}>
                    <Field.Label>{t("auth_page_password")}</Field.Label>
                    <Input type="password" {...register("password")} autoComplete="off" placeholder={t("auth_page_password")} />
                    <Field.ErrorText>{errors.password?.message}</Field.ErrorText>
                </Field.Root>
                {error && <div className="auth-error" style={{ color: "#e53e3e", margin: "1rem 0", textAlign: "center" }}>{error}</div>}
                <Button loading={loading} type="submit" colorScheme="teal" w="full" mt={4} size="lg">{t("auth_page_login")}</Button>
            </Box>
        </Flex>
    );
};

export default AuthPage;
