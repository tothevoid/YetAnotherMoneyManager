import { createSystem, defaultConfig, defineRecipe } from "@chakra-ui/react"

const inputRecipe = defineRecipe({
    variants: {
        variant: {
            outline: {
                borderColor: "border_primary",
                backgroundColor: "background_primary",
                color: "text_primary",
                _hover: {
                    borderColor: "border_primary",
                },
                _focusVisible: {
                    borderColor: "action_primary",
                    boxShadow: "0 0 0 1px {colors.action_primary}",
                },
            },
        },
    },
})

export const darkTheme = createSystem(defaultConfig, {
    theme: {
        tokens: {
            colors: {
                background_main: { value: "#121212" },
                text_primary: { value: "#E0E0E0"},
                text_secondary: { value: "#A0A0A0"},
                background_primary: {value: "#242424"},
                border_primary: {value: "rgba(255, 255, 255, 0.1)"},
                background_secondary: {value: "#1E1E1E"},
                card_action_icon_primary: {"value": "#f3e8ff"},
                card_action_icon_danger: {"value": "#dc2626"},
                header_bg: {"value": "#181818"},
                action_primary: {value: "#0a8e3a"},
                spinner_primary: {value: "#0a8e3a"},

                // Transactions
                buy_action_bg: {value: "#0a8e3a"},
                sell_action_bg: {value: "#dc2626"},

                // Stats
                gain: {value: "#0a8e3a"},
                loss: {value: "#dc2626"},

                // Status / Feedback
                status_success: { value: "#4ade80" },
                status_success_bg: { value: "rgba(34, 197, 94, 0.12)" },
                status_success_border: { value: "rgba(34, 197, 94, 0.3)" },
                status_danger: { value: "#f87171" },
                status_danger_bg: { value: "rgba(239, 68, 68, 0.12)" },
                status_danger_border: { value: "rgba(239, 68, 68, 0.3)" },

                // P&L
                pnl_positive: { value: "#4ade80" },
                pnl_positive_bg: { value: "rgba(34, 197, 94, 0.12)" },
                pnl_positive_border: { value: "rgba(34, 197, 94, 0.3)" },
                pnl_negative: { value: "#f87171" },
                pnl_negative_bg: { value: "rgba(239, 68, 68, 0.12)" },
                pnl_negative_border: { value: "rgba(239, 68, 68, 0.3)" },
            },
        },
        semanticTokens: {
            colors: {
                border: {
                    value: "{colors.border_primary}",
                },
            },
        },
        recipes: {
            input: inputRecipe,
        },
    },
})
