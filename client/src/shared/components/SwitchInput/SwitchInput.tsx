import React from 'react';
import { Switch, SwitchRootProps } from '@chakra-ui/react';

export interface SwitchInputProps extends Omit<SwitchRootProps, 'checked' | 'onCheckedChange'> {
    checked?: boolean;
    onCheckedChange?: (checked: boolean) => void;
    label?: string;
    colorPalette?: string;
    size?: 'sm' | 'md' | 'lg';
    disabled?: boolean;
    onClick?: (e: React.MouseEvent) => void;
}

export const SwitchInput: React.FC<SwitchInputProps> = ({
    checked,
    onCheckedChange,
    label,
    colorPalette = 'green',
    size = 'sm',
    disabled,
    onClick,
    ...rest
}) => {
    return (
        <Switch.Root
            checked={checked}
            disabled={disabled}
            colorPalette={colorPalette}
            size={size}
            onCheckedChange={(details) => onCheckedChange?.(!!details.checked)}
            onClick={onClick}
            {...rest}
        >
            <Switch.HiddenInput />
            <Switch.Control>
                <Switch.Thumb />
            </Switch.Control>
            {label && <Switch.Label color="text_primary">{label}</Switch.Label>}
        </Switch.Root>
    );
};

export default SwitchInput;
