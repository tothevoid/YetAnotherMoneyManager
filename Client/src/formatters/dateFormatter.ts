export enum DateFormat {
	RU = 0,
	EN = 1
}

const localeMapping = new Map<DateFormat, string>([
	[DateFormat.RU, "ru-RU"],
	[DateFormat.EN, "en-US"],
]);

export const formatDate = (date: Date, showYear: boolean = true, 
	format: DateFormat = DateFormat.RU) => {
	const locale = localeMapping.get(format);
	return new Intl.DateTimeFormat(locale, {
		year: showYear ? "numeric" : undefined,
		month: "long",
		day: "numeric"
	}).format(date);
};

export const formatNumericDate = (date: Date, showYear: boolean = true, 
	format: DateFormat = DateFormat.RU) => {
	const locale = localeMapping.get(format);
	return new Intl.DateTimeFormat(locale, {
		year: showYear ? "2-digit" : undefined,
		month: "numeric",
		day: "numeric"
	}).format(date);
};