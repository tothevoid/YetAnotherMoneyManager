import { i18n } from "i18next";

export const getMonthByIndex = (index: number, i18n: i18n) => 
    getMonthsNames(i18n)[index - 1];

export const convertToDateOnly = (date: Date) => {
    const year = date.getFullYear().toString();
    const month = (date.getMonth() + 1).toString().padStart(2, "0");
    const day = date.getDate().toString().padStart(2, "0");
    return `${year}-${month}-${day}`;
}

export const getMonthsNames = (i18n: i18n) => {
    const formatter = new Intl.DateTimeFormat(i18n.language, { month: 'long' });

     return Array.from({ length: 12 }, (_, i) => 
        formatter.format(new Date(2000, i, 1)).slice(0, 3)
    );
}
   
export const getCurrentDate = (): string => {
    const date = new Date();
    return new Date(date.getTime() - (date.getTimezoneOffset() * 60000)).toISOString().substr(0, 10);
}