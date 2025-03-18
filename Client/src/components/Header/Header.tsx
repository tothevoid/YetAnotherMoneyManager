import './Header.scss';
import HeaderItem from '../HeaderItem/HeaderItem';
import { Button, Spacer } from '@chakra-ui/react';
import { changeLanguage } from 'i18next';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

const currencyMapping = new Map<string, string>([
    ["en-US", "EN"],
    ["ru-RU", "RU"],
]);

const getLanguageCaption = (currentCode: string) => {
    return currencyMapping.get(currentCode) ?? ""
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

    return <div className="header">
        <HeaderItem title={t("header_accounts")} path='/'></HeaderItem>
        <HeaderItem title={t("header_transactions")} path='/transactions'></HeaderItem>
        <HeaderItem title={t("header_deposits")} path='/deposits'></HeaderItem>
        <Spacer/>
        <Button onClick={() => updateLanguage()} background={"purple.600"}>
            {language}
        </Button>
    </div>
}
   

export default Header;