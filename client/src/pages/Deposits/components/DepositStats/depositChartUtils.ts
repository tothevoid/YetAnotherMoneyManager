import { i18n } from "i18next";

export const formatPeriodLabel = (
    period: Date | string | null | undefined,
    format: i18n,
    mode: "short" | "full" = "short"
): string => {
    if (!period) {
        return "";
    }

    const date = period instanceof Date ? period : new Date(period);
    if (isNaN(date.getTime())) {
        return typeof period === "string" ? period : "";
    }

    const lang = format?.language || "en";
    const fullYear = date.getFullYear();

    if (mode === "short") {
        const monthName = date.toLocaleString(lang, { month: "short" });
        const capitalizedMonth = monthName.charAt(0).toUpperCase() + monthName.slice(1);
        const shortYear = fullYear.toString().slice(-2);
        return `${capitalizedMonth} '${shortYear}`;
    }

    const monthName = date.toLocaleString(lang, { month: "long" });
    const capitalizedMonth = monthName.charAt(0).toUpperCase() + monthName.slice(1);
    return `${capitalizedMonth} ${fullYear}`;
};
