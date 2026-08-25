import React, { useState } from 'react';
import { Button, Icon } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { MdCheck, MdContentCopy } from 'react-icons/md';

export interface CopyButtonProps {
    text: string;
    title?: string;
    copiedTitle?: string;
    size?: '2xs' | 'xs' | 'sm' | 'md' | 'lg';
    variant?: 'ghost' | 'outline' | 'solid' | 'subtle';
    color?: string;
    showLabel?: boolean;
    durationMs?: number;
    onClick?: (e: React.MouseEvent) => void;
}

export const CopyButton: React.FC<CopyButtonProps> = ({
    text,
    title,
    copiedTitle,
    size = 'xs',
    variant = 'ghost',
    color,
    showLabel = true,
    durationMs = 2000,
    onClick
}) => {
    const { t } = useTranslation();
    const [isCopied, setIsCopied] = useState<boolean>(false);

    const handleCopy = async (e: React.MouseEvent) => {
        e.stopPropagation();
        onClick?.(e);
        if (!text) return;

        try {
            await navigator.clipboard.writeText(text);
            setIsCopied(true);
            setTimeout(() => setIsCopied(false), durationMs);
        } catch {
            // ignore clipboard permissions/errors
        }
    };

    const defaultTitle = title ?? t('copy');
    const defaultCopiedTitle = copiedTitle ?? t('copied');

    return (
        <Button
            size={size}
            variant={variant}
            color={color}
            onClick={handleCopy}
            title={isCopied ? defaultCopiedTitle : defaultTitle}
        >
            <Icon mr={showLabel ? 1 : 0}>
                {isCopied ? <MdCheck /> : <MdContentCopy />}
            </Icon>
            {showLabel && (isCopied ? defaultCopiedTitle : defaultTitle)}
        </Button>
    );
};

export default CopyButton;
