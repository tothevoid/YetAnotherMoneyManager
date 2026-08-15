export const formatCurrency = (
    amount: number,
    currencySymbol: string = '₽',
    locale: string = 'ru-RU'
): string => {
    const formatted = new Intl.NumberFormat(locale, {
        minimumFractionDigits: 0,
        maximumFractionDigits: 2,
    }).format(amount);

    return currencySymbol ? `${formatted} ${currencySymbol}` : formatted;
};
