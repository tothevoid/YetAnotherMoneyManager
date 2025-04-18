import { createSystem, defaultConfig } from "@chakra-ui/react"

export const darkTheme = createSystem(defaultConfig, {
    theme: {
        tokens: {
            colors: {
                background_main: { value: "#121212" },
                text_primary: { value: "#E0E0E0"},
                background_primary: {value: "#242424"},
                border_primary: {value: "rgba(255, 255, 255, 0.1)"},
                background_secondary: {value: "#1E1E1E"},
                card_action_icon_primary: {"value": "#f3e8ff"},
                card_action_icon_danger: {"value": "#dc2626"},
                header_bg: {"value": "#181818"}
            },
        },
    },
})
