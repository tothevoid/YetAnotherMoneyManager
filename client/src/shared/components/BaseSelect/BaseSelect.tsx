import { ChakraStylesConfig, Select } from "chakra-react-select";

export type ValueType<T, IsClearable extends boolean> = IsClearable extends true ? T | null : T;

export interface BaseSelectProps<T, IsClearable extends boolean = false> {
    placeholder?: string;
    collection: readonly T[] | T[];
    selectedValue?: T | null | undefined;
    labelSelector: (item: T) => string;
    valueSelector: (item: T) => string | number;
    onSelected: (item: ValueType<T, IsClearable>) => void;
    isDisabled?: boolean;
    isClearable?: IsClearable;
}

export const selectChakraStyles: ChakraStylesConfig = {
    control: (provided) => ({
        ...provided,
        backgroundColor: "background_primary",
        borderColor: "border_primary",
        color: "text_primary",
        _hover: {
            borderColor: "border_primary",
        },
    }),
    option: (provided, state) => ({
        ...provided,
        color: state.isSelected ? "white" : "text_primary",
        backgroundColor: state.isSelected
            ? "action_primary"
            : state.isFocused
                ? "background_secondary"
                : "background_primary",
    }),
    singleValue: (provided) => ({
        ...provided,
        color: "text_primary",
    }),
    menuList: (provided) => ({
        ...provided,
        backgroundColor: "background_primary",
        borderColor: "border_primary",
        boxShadow: "md",
        borderRadius: "8px",
    }),
    placeholder: (provided) => ({
        ...provided,
        color: "gray.500",
    }),
};

const BaseSelect = <T, IsClearable extends boolean = false>({
    placeholder,
    selectedValue,
    onSelected,
    collection = [],
    labelSelector,
    valueSelector,
    isDisabled,
    isClearable = false as IsClearable,
}: BaseSelectProps<T, IsClearable>) => {
    return (
        <Select<T, false>
            isDisabled={isDisabled}
            chakraStyles={selectChakraStyles as ChakraStylesConfig<T, false>}
            getOptionLabel={labelSelector}
            getOptionValue={(item) => String(valueSelector(item))}
            options={collection as T[]}
            value={selectedValue}
            isClearable={isClearable}
            onChange={(option) => onSelected(option as ValueType<T, IsClearable>)}
            placeholder={placeholder ?? ""}
        />
    );
};

export default BaseSelect;


