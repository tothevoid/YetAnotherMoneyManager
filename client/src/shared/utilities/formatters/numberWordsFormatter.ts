export const formatNumberToWords = (num: number, locale: string = 'ru-RU'): string => {
    if (!num || isNaN(num)) return '';
    const isEn = locale.startsWith('en');

    if (num >= 1_000_000_000) {
        const value = (num / 1_000_000_000).toLocaleString(locale, { maximumFractionDigits: 2 });
        return isEn ? `${value}B` : `${value} млрд`;
    }
    if (num >= 1_000_000) {
        const value = (num / 1_000_000).toLocaleString(locale, { maximumFractionDigits: 2 });
        return isEn ? `${value}M` : `${value} млн`;
    }
    if (num >= 1_000) {
        const value = (num / 1_000).toLocaleString(locale, { maximumFractionDigits: 1 });
        return isEn ? `${value}k` : `${value} тыс.`;
    }
    return num.toString();
};
