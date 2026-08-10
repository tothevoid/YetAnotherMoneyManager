import { Checkbox, ConditionalValue } from "@chakra-ui/react";
import { Control, Controller, FieldValues, Path } from "react-hook-form"

interface Props<TFieldValues extends FieldValues> {
    name: Path<TFieldValues>,
    title: string,
    control: Control<TFieldValues>
    variant?: ConditionalValue<"outline" | "solid" | "subtle" | undefined>
}

const CheckboxInput = <TFieldValues extends FieldValues>({name, title, control, variant = "solid"}: Props<TFieldValues>) => {
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