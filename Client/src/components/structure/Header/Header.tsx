import HeaderItem from '../HeaderItem/HeaderItem';
import { Box, Button,Flex, Icon, Text } from '@chakra-ui/react';
import { useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { MdOutlineSettings } from 'react-icons/md';
import UserProfileSettingsModal, { UserProfileSettingsModalRef } from '../../../modals/UserProfileSettingsModal/UserProfileSettingsModal';

const Header = () => {
    const { t } = useTranslation();
    const userProfileSettingsRef = useRef<UserProfileSettingsModalRef>(null);

    const onOpenSettingsClick = () => {
        userProfileSettingsRef.current?.openModal();
    }

    return ( 
        <Box pos="fixed" w={"100%"} zIndex={1000}>
            <Flex minH={50} alignItems="center" padding={1} direction={'row'} backgroundColor="header_bg" color="text_primary">
                <Flex flex={{ base: 1 }} justify="center" align={"center"}>
                    <Text fontWeight={900} textAlign="center">
                       YAMM
                    </Text>
                    <Flex flex="1">
                        <HeaderItem title={t("header_accounts")} path='/'></HeaderItem>
                        <HeaderItem title={t("header_transactions")} path='/transactions'></HeaderItem>
                        <HeaderItem title={t("header_deposits")} path='/deposits'></HeaderItem>
                        <HeaderItem title={t("header_broker_account")} path='/broker_accounts'></HeaderItem>
                        <HeaderItem title={t("header_securities")} path='/securities'></HeaderItem>
                        <HeaderItem title={t("header_data")} path='/data'></HeaderItem>
                    </Flex>
                </Flex>
                <Flex flex="1" justify="flex-end" direction="row">
                    <Button borderColor="background_secondary" background="button_background_secondary" size={'2xl'} onClick={onOpenSettingsClick}>
                        <Icon color="card_action_icon_primary">
                            <MdOutlineSettings/>
                        </Icon>
                    </Button>
                </Flex>
                <UserProfileSettingsModal ref={userProfileSettingsRef}/>
            </Flex>
        </Box>)
}
   

export default Header;
