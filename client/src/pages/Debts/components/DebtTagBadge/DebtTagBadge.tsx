import React from "react";
import { Badge, BadgeProps } from "@chakra-ui/react";

export interface DebtTagBadgeProps extends Omit<BadgeProps, "name"> {
    name: string;
    colorHex: string;
}

export const DebtTagBadge: React.FC<DebtTagBadgeProps> = ({
    name,
    colorHex,
    px = 2.5,
    py = 0.5,
    borderRadius = "full",
    fontSize = "xs",
    fontWeight = "semibold",
    children,
    style,
    ...restProps
}) => {
    return (
        <Badge
            style={{
                backgroundColor: `${colorHex}26`,
                color: colorHex,
                border: `1px solid ${colorHex}50`,
                ...style,
            }}
            px={px}
            py={py}
            borderRadius={borderRadius}
            fontSize={fontSize}
            fontWeight={fontWeight}
            {...restProps}
        >
            {name}
            {children}
        </Badge>
    );
};

export default DebtTagBadge;
