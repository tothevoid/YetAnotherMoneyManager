
import { useState } from "react";
import { Box, Flex } from "@chakra-ui/react";
import "./AuthPage.scss";
import { useLocation, useNavigate } from "react-router-dom";
import AuthForm from "./components/AuthForm/AuthForm";
import ChangePasswordForm from "./components/ChangePasswordForm/ChangePasswordForm";
import { Nullable } from "../../shared/utilities/nullable";

enum FormType {
	Auth,
	ChangePassword
}

const AuthPage: React.FC = () => {
    const [formType, setFormType] = useState<FormType>(FormType.Auth);

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from ?? "/";

    const onTokenReceived = (token: string) => {
        localStorage.setItem("auth_token", token);
        navigate(from, { replace: true });
    }

    const [defaultPasswordResetValues, setDefaultPasswordResetValues] = 
        useState<{userName: string, currentPassword: Nullable<string>}>({userName: "", currentPassword: null});

    const onPasswordChangeRequired = (userName: string, currentPassword: Nullable<string>) => {
        setDefaultPasswordResetValues({ userName, currentPassword });
        setFormType(FormType.ChangePassword);
    }

    return (
        <Flex minH="100vh" w="100vw" align="center" justify="center" 
            bgGradient="linear(120deg, #e0eafc 0%, #cfdef3 100%)">
            <Box
                maxW="sm"
                w="100%"
                p={8}
                borderRadius="xl"
                boxShadow="lg"
                bg="white"
            >
            {formType === FormType.Auth && <AuthForm onPasswordChangeRequired={onPasswordChangeRequired} onTokenReceived={onTokenReceived} />}
            {formType === FormType.ChangePassword && <ChangePasswordForm defaultPasswordResetValues={defaultPasswordResetValues} onTokenReceived={onTokenReceived} />}
            </Box>
        </Flex>
    );
};

export default AuthPage;
