import { Box, Input, InputProps, Text } from "@chakra-ui/react";
import React from "react";
import { UseFormRegisterReturn } from "react-hook-form";

interface MoneyInputProps extends Omit<InputProps, "register"> {
    register?: UseFormRegisterReturn;
    currency?: string;
}

const MoneyInput: React.FC<MoneyInputProps> = ({
    register,
    currency,
    placeholder = "500",
    min = 0,
    step = "0.01",
    type = "number",
    ...rest
}) => {
    return (
        <Box position="relative" width="100%">
            <Input
                {...register}
                {...rest}
                min={min}
                step={step}
                autoComplete="off"
                type={type}
                placeholder={placeholder}
                color="text_primary"
                backgroundColor="background_primary"
                borderColor="border_primary"
                pr={currency ? "60px" : "12px"}
            />
            {currency && (
                <Text
                    position="absolute"
                    right="12px"
                    top="50%"
                    transform="translateY(-50%)"
                    color="gray.400"
                    fontSize="sm"
                    fontWeight="medium"
                    pointerEvents="none"
                    userSelect="none"
                >
                    {currency}
                </Text>
            )}
        </Box>
    );
};

export default MoneyInput;
