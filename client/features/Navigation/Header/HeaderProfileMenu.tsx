import React, { useState } from 'react';
import { Popover } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { useUserProfile } from '../../UserProfileSettingsModal/hooks/UserProfileContext';
import { logoutApi, revokeAllApi } from '../../../src/api/auth/authApi';
import { HeaderProfileButton } from './HeaderProfileButton';
import { HeaderProfileCard } from './HeaderProfileCard';

interface HeaderProfileMenuProps {
    onOpenSettings: () => void;
    onOpenChangePassword: () => void;
}

export const HeaderProfileMenu: React.FC<HeaderProfileMenuProps> = ({
    onOpenSettings,
    onOpenChangePassword
}) => {
    const { t } = useTranslation();
    const { user } = useUserProfile();
    const [open, setOpen] = useState(false);

    const handleOpenSettings = () => {
        setOpen(false);
        onOpenSettings();
    };

    const handleOpenChangePassword = () => {
        setOpen(false);
        onOpenChangePassword();
    };

    const handleLogout = async () => {
        setOpen(false);
        await logoutApi();
    };

    const handleRevokeAll = async () => {
        setOpen(false);
        await revokeAllApi();
    };

    const userName = user?.userName || t('header_profile_user');
    const userInitial = userName.charAt(0).toUpperCase();

    return (
        <Popover.Root open={open} onOpenChange={e => setOpen(e.open)} positioning={{ placement: 'bottom-end' }}>
            <Popover.Trigger asChild>
                <HeaderProfileButton
                    userName={userName}
                    userInitial={userInitial}
                    isOpen={open}
                />
            </Popover.Trigger>
            <HeaderProfileCard
                userName={userName}
                userInitial={userInitial}
                onOpenSettings={handleOpenSettings}
                onOpenChangePassword={handleOpenChangePassword}
                onLogout={handleLogout}
                onRevokeAll={handleRevokeAll}
            />
        </Popover.Root>
    );
};
