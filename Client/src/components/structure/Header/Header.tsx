import HeaderItem from '../HeaderItem/HeaderItem';
import { Box, Button,Flex, Text } from '@chakra-ui/react';
import { changeLanguage } from 'i18next';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

const langMapping = new Map<string, string>([
    ["en-US", "EN"],
    ["ru-RU", "RU"],
]);

const getLanguageCaption = (currentCode: string) => {
    return langMapping.get(currentCode) ?? ""
}

const Header = () => {
    const { t, i18n } = useTranslation();
    const [language, setLanguage] = useState(getLanguageCaption(i18n.language));

    const updateLanguage = () => {
        const nextLanguage = i18n.language === "en-US" ? "ru-RU" : "en-US";
        setLanguage(getLanguageCaption(nextLanguage));
        localStorage.setItem("lang", nextLanguage);
        changeLanguage(nextLanguage)
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
                    <Button onClick={() => updateLanguage()} background={"purple.600"}>
                        {language}
                    </Button>
                </Flex>
            </Flex>
        </Box>)
}
   

export default Header;
