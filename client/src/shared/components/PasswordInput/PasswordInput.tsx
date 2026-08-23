import { forwardRef, useState } from 'react';
import { Box, Flex, IconButton, Input, InputProps } from '@chakra-ui/react';
import { MdVisibility, MdVisibilityOff } from 'react-icons/md';

export interface PasswordInputProps extends Omit<InputProps, 'type'> {
    disableAutofill?: boolean;
}

export const PasswordInput = forwardRef<HTMLInputElement, PasswordInputProps>(({
    disableAutofill = false,
    autoComplete,
    size = 'sm',
    ...rest
}, ref) => {
    const [showPassword, setShowPassword] = useState(false);

    const autofillProps = disableAutofill ? {
        autoComplete: 'off',
        'data-lpignore': 'true',
        'data-1p-ignore': 'true',
        'data-bwignore': 'true',
        'data-form-type': 'other',
    } : {
        autoComplete: autoComplete ?? 'current-password',
    };

    return (
        <Box position="relative" width="100%">
            <Input
                ref={ref}
                type={showPassword ? 'text' : 'password'}
                size={size}
                backgroundColor="background_secondary"
                borderColor="border_primary"
                color="text_primary"
                pr="36px"
                {...autofillProps}
                {...rest}
            />
            <Flex
                position="absolute"
                right="4px"
                top="50%"
                transform="translateY(-50%)"
                align="center"
                zIndex={1}
            >
                <IconButton
                    aria-label={showPassword ? 'Hide password' : 'Show password'}
                    variant="ghost"
                    size="xs"
                    color="gray.400"
                    _hover={{ color: 'text_primary', bg: 'transparent' }}
                    onClick={() => setShowPassword(!showPassword)}
                    tabIndex={-1}
                >
                    {showPassword ? <MdVisibilityOff size={16} /> : <MdVisibility size={16} />}
                </IconButton>
            </Flex>
        </Box>
    );
});

PasswordInput.displayName = 'PasswordInput';
