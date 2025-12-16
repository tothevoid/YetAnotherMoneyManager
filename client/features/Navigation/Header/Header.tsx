import HeaderItem from '../HeaderItem/HeaderItem';
import { Box, Button,Flex, Icon, Image, Link } from '@chakra-ui/react';
import { useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { MdOutlineSettings } from 'react-icons/md';
import UserProfileSettingsModal from '../../UserProfileSettingsModal/UserProfileSettingsModal';
import { NavLink, useNavigate } from 'react-router-dom';
import { BaseModalRef } from '../../../src/shared/utilities/modalUtilities';
import { MdOutlineExitToApp } from "react-icons/md";

import appIcon from './AppIcon.svg';

const Header = () => {
    const { t } = useTranslation();
    const userProfileSettingsRef = useRef<BaseModalRef>(null);

    const navigate = useNavigate();

    const onOpenSettingsClick = () => {
        userProfileSettingsRef.current?.openModal();
    }

    const onExitClick = () => {
        localStorage.setItem("auth_token", "");
        navigate("/auth", { replace: true });
    }

    const tabs = [
        { path: "/", title: t("header_dashboard")},
        { path: "accounts", title: t("header_accounts")},
        { path: "transactions", title: t("header_transactions")},
        { path: "deposits", title: t("header_deposits")},
        { path: "broker_accounts", title: t("header_broker_account")},
        { path: "securities", title: t("header_securities")},
        { path: "debts", title: t("header_debts")},
        { path: "cryptocurrencies", title: t("header_cryptocurrencies")},
        { path: "crypto_accounts", title: t("header_cryptoaccounts")},
        { path: "data", title: t("header_data")}
    ]

    return <nav>
        <Box pos="fixed" w={"100%"} zIndex={1000}>
            <Flex minH={50} alignItems="center" padding={1} direction={'row'} backgroundColor="header_bg" color="text_primary">
                <Flex flex={{ base: 1 }} justify="center" align={"center"}>
                    <Link href='/'>
                        <Image marginInline={"10px"} width="30px" src={appIcon}></Image>
                    </Link>
                      <Flex flex="1">
                        {
                            tabs.map(tab => 
                                <NavLink key={tab.path} to={tab.path} className={({ isActive }) => isActive ? 'active' : ''}>
                                    {({ isActive }) => <HeaderItem title={tab.title} active={isActive} />}
                                </NavLink>
                            )
                        }
                    </Flex>
                </Flex>
                <Flex width="50px" justify="flex-end" direction="row">
                    <Button borderColor="background_secondary" background="button_background_secondary" size={'md'} onClick={onOpenSettingsClick}>
                        <Icon color="card_action_icon_primary">
                            <MdOutlineSettings/>
                        </Icon>
                    </Button>
                    <Button borderColor="background_secondary" background="button_background_secondary" size={'md'} onClick={onExitClick}>
                        <Icon color="card_action_icon_primary">
                            <MdOutlineExitToApp/>
                        </Icon>
                    </Button>
                </Flex>
                <UserProfileSettingsModal ref={userProfileSettingsRef}/>
            </Flex>
        </Box>
    </nav>
}

export default Header;
