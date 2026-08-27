import type { i18n } from 'i18next';

export type SupportedLanguage = 'ru' | 'en';

/**
 * Normalizes i18n language string (e.g. 'ru-RU', 'RU', 'en-US') to standard 2-letter language code ('ru' | 'en').
 */
export const getNormalizedLanguage = (i18nOrLang?: i18n | string | null): SupportedLanguage => {
    const rawLang = typeof i18nOrLang === 'string'
        ? i18nOrLang
        : i18nOrLang?.language;

    const clean = (rawLang ?? 'en').trim().toLowerCase();
    if (clean.startsWith('ru')) {
        return 'ru';
    }
    return 'en';
};
