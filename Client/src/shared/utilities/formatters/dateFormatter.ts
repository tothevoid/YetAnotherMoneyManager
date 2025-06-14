import { i18n } from "i18next";

export const formatDate = (date: Date, format: i18n, showYear: boolean = true, ) => {
	return new Intl.DateTimeFormat(format.language, {
		year: showYear ? "numeric" : undefined,
		month: "long",
		day: "numeric"
	}).format(date);
};

export const formatDateTime = (date: Date, format: i18n, showYear: boolean = true) => {
	return new Intl.DateTimeFormat(format.language, {
		year: showYear ? "numeric" : undefined,
		month: "long",
		day: "numeric",
		hour: "2-digit",
		minute: "2-digit"
	}).format(date);
};

export const formatNumericDate = (date: Date, format: i18n, showYear: boolean = true) => {
	return new Intl.DateTimeFormat(format.language, {
		year: showYear ? "2-digit" : undefined,
		month: "numeric",
		day: "numeric"
	}).format(date);
};

export const formatMonth = (month: number, format: i18n): string => {
	const date = new Date();
	date.setMonth(month - 1);
	return date.toLocaleString(format.language, { month: 'long' });
}