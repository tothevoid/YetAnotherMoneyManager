import React from "react";
import { Badge, BadgeProps } from "@chakra-ui/react";

export interface DebtTagBadgeProps extends Omit<BadgeProps, "name"> {
    name: string;
    colorHex?: string;
    isSelected?: boolean;
}

export const DebtTagBadge: React.FC<DebtTagBadgeProps> = ({
    name,
    colorHex,
    isSelected,
    px = 2.5,
    py = 0.5,
    borderRadius = "full",
    fontSize = "xs",
    fontWeight,
    children,
    style,
    ...restProps
}) => {
    const defaultBg = colorHex
        ? (isSelected !== undefined
            ? (isSelected ? `${colorHex}40` : `${colorHex}1A`)
            : `${colorHex}26`)
        : (isSelected !== undefined
            ? (isSelected ? "action_primary" : "transparent")
            : "transparent");

    const defaultColor = colorHex
        ? colorHex
        : (isSelected ? "white" : "text_primary");

    const defaultBorder = colorHex
        ? (isSelected !== undefined
            ? `1px solid ${colorHex}${isSelected ? "80" : "40"}`
            : `1px solid ${colorHex}50`)
        : (isSelected
            ? "1px solid var(--chakra-colors-action_primary)"
            : "1px solid var(--chakra-colors-border_primary)");

    const defaultOutline = isSelected
        ? (colorHex ? `2px solid ${colorHex}` : "1px solid var(--chakra-colors-action_primary)")
        : "none";

    const defaultOpacity = colorHex ? undefined : (isSelected ? 1 : 0.85);

    const defaultFontWeight = fontWeight || (isSelected !== undefined ? (isSelected ? "bold" : "normal") : "semibold");

    return (
        <Badge
            variant="plain"
            style={{
                backgroundColor: defaultBg,
                color: defaultColor,
                border: defaultBorder,
                outline: defaultOutline,
                opacity: defaultOpacity,
                ...style,
            }}
            px={px}
            py={py}
            borderRadius={borderRadius}
            fontSize={fontSize}
            fontWeight={defaultFontWeight}
            {...restProps}
        >
            {name}
            {children}
        </Badge>
    );
};

export default DebtTagBadge;
