import { Button, HStack } from '@chakra-ui/react';

export interface ButtonGroupOption<T> {
    value: T;
    label: string;
}

export interface ButtonGroupProps<T> {
    options: ButtonGroupOption<T>[];
    value?: T;
    values?: T[];
    isMulti?: boolean;
    onChange?: (value: T) => void;
    onToggle?: (value: T) => void;
    size?: '2xs' | 'xs' | 'sm' | 'md' | 'lg';
    wrap?: 'wrap' | 'nowrap';
    gap?: number | string;
}

export function ButtonGroup<T extends string | number | boolean>({
    options,
    value,
    values,
    isMulti = false,
    onChange,
    onToggle,
    size = 'sm',
    wrap = 'wrap',
    gap = 2
}: ButtonGroupProps<T>) {
    return (
        <HStack gap={gap} wrap={wrap}>
            {options.map((opt) => {
                const isSelected = isMulti
                    ? values?.includes(opt.value)
                    : opt.value === value;

                const handleClick = () => {
                    if (isMulti) {
                        onToggle?.(opt.value);
                    } else {
                        onChange?.(opt.value);
                    }
                };

                return (
                    <Button
                        key={String(opt.value)}
                        size={size}
                        variant={isSelected ? 'solid' : 'outline'}
                        background={isSelected ? 'action_primary' : 'transparent'}
                        color="text_primary"
                        borderColor="border_primary"
                        _hover={{
                            background: isSelected ? 'action_primary' : 'background_secondary',
                            borderColor: 'border_secondary'
                        }}
                        onClick={handleClick}
                    >
                        {opt.label}
                    </Button>
                );
            })}
        </HStack>
    );
}

export default ButtonGroup;
