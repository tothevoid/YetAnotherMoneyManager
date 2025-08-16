import { Checkbox, ConditionalValue } from "@chakra-ui/react";
import { Control, Controller } from "react-hook-form"

interface Props {
    name: string,
    title: string,
    control: Control<any>
    variant?: ConditionalValue<"outline" | "solid" | "subtle" | undefined>
}

const CheckboxInput: React.FC<Props> = ({name, title, control, variant = "solid"}) => {
    return <Controller
        name={name}
        control={control}
        render={({ field: {onChange, value} }) => (
            <Checkbox.Root checked={value} onCheckedChange={(data) => {onChange(data.checked)}} variant={variant}>
                <Checkbox.HiddenInput />
                <Checkbox.Control />
                <Checkbox.Label>{title}</Checkbox.Label>
            </Checkbox.Root>
        )}
    />
}

export default CheckboxInput;