import './Header.scss';
import HeaderItem from '../HeaderItem/HeaderItem';
import { Button, Spacer } from '@chakra-ui/react';
import { changeLanguage } from 'i18next';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

const Header = () => {
    const { t, i18n } = useTranslation();
    const [language, setLanguage] = useState(i18n.language);

    const updateLanguage = () => {
        setLanguage(i18n.language);
        const nextLanguage = i18n.language === "en" ? "ru" : "en";
        localStorage.setItem("lang", nextLanguage);
        changeLanguage(nextLanguage)
    }

    return <div className="header">
        <HeaderItem title={t("title_manager")} path='/'></HeaderItem>
        <HeaderItem title={t("title_deposits")} path='/Deposits'></HeaderItem>
        <Spacer/>
        <Button onClick={() => updateLanguage()} background={"gray.400"}>
            {language}
        </Button>
    </div>
}
   

export default Header;