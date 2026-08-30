/**
 * Shared color palettes and semantic colors for Recharts across the application.
 * Tailored for Audex dark theme (#121212 / #1E1E1E / #242424).
 */

export const CHARTS_COLORS = [
    "#38bdf8", // Sky blue
    "#818cf8", // Indigo
    "#34d399", // Emerald
    "#fbbf24", // Amber
    "#f472b6", // Pink
    "#a78bfa", // Purple
    "#2dd4bf", // Teal
    "#fb923c", // Orange
    "#60a5fa", // Blue
    "#e879f9", // Fuchsia
    "#4ade80", // Light green
    "#f87171", // Coral red
    "#c084fc", // Violet
    "#38d9a9", // Mint
    "#facc15", // Yellow
    "#ec4899", // Rose
];

export const CHART_THEME_COLORS = {
    earnings: "#10b981", // Emerald accent for positive earnings / yield
    positive: "#4ade80", // Green for profits / gains
    negative: "#f87171", // Red for losses / expenses
    grid: "rgba(255, 255, 255, 0.07)",
    axisLine: "rgba(255, 255, 255, 0.1)",
    axisText: "var(--chakra-colors-text_secondary)",
    cursorStroke: "rgba(255, 255, 255, 0.15)",
    cursorFill: "rgba(255, 255, 255, 0.05)",
    tooltipBg: "#1E1E1E",
    tooltipBorder: "rgba(255, 255, 255, 0.15)",
    divider: "rgba(255, 255, 255, 0.08)",
};

export const getChartColor = (index: number): string => {
    return CHARTS_COLORS[index % CHARTS_COLORS.length];
};
