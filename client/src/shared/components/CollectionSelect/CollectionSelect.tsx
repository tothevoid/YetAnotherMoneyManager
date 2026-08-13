import { Select } from "chakra-react-select";
import { Control, Controller, FieldValues, Path } from "react-hook-form"

// eslint-disable-next-line @typescript-eslint/no-explicit-any
type AnySelect = typeof Select<any>

interface Props<T, TFieldValues extends FieldValues> {
    name: Path<TFieldValues>
    placeholder?: string
    control: Control<TFieldValues>
    collection: T[],
    labelSelector: (item: T) => string,
    valueSelector: (item: T) => string,
}

//TODO: Fix generics for lambdas
const CollectionSelect = <T, TFieldValues extends FieldValues>({ name, placeholder, control, collection = [], labelSelector, valueSelector }: Props<T, TFieldValues>) => {
    const AnySelect = Select as AnySelect
    return <Controller
        name={name}
        control={control}
        render={({ field }) => (
            <AnySelect
                {...field}
                chakraStyles={{
                    control: (provided: any) => ({
                        ...provided,
                        backgroundColor: "background_primary",
                        borderColor: "border_primary",
                        color: "text_primary",
                        _hover: {
                            borderColor: "border_primary",
                        },
                    }),
                    option: (provided: any, state: any) => ({
                        ...provided,
                        color: state.isSelected ? "white" : "text_primary",
                        backgroundColor: state.isSelected
                            ? "action_primary"
                            : state.isFocused
                                ? "background_secondary"
                                : "background_primary",
                    }),
                    singleValue: (provided: any) => ({
                        ...provided,
                        color: "text_primary",
                    }),
                    menuList: (provided: any) => ({
                        ...provided,
                        backgroundColor: "background_primary",
                        borderColor: "border_primary",
                        boxShadow: "md",
                        borderRadius: "8px",
                    }),
                    placeholder: (provided: any) => ({
                        ...provided,
                        color: "gray.500",
                    }),
                }}
                getOptionLabel={labelSelector}
                getOptionValue={valueSelector}
                options={collection}
                isClearable
                placeholder={placeholder ?? ""}>
            </AnySelect>
        )}
    />
}

export default CollectionSelect;
