import { Select } from "chakra-react-select";

interface Props<T> {
    placeholder?: string
    collection: T[],
    selectedValue: T,
    labelSelector: (item: T) => string,
    valueSelector: (item: T) => string,
    onSelected: (item: T) => void,
}

//TODO: Fix generics for lambdas
type CollectionSelectProps<T = any> = React.FC<Props<T>>

const BaseSelect: CollectionSelectProps = ({placeholder, selectedValue, onSelected, collection = [], labelSelector, valueSelector}) => {
    return <Select
        chakraStyles={{
            option: (provided) => ({
                ...provided,
                color: "text_primary",
                bg: "background_primary"
            }),
            singleValue: (provided) => ({
                ...provided,
                color: "text_primary",
                marginLeft: "10px"
            }),
            menuList: (provided) => ({
                ...provided,
                bg: "background_primary",
                border: "1px solid white",
                borderColor: "border_primary",
                boxShadow: "md",
                borderRadius: "8px"
            })
        }}

        getOptionLabel={labelSelector}
        getOptionValue={valueSelector}
        options={collection}
        value={selectedValue}
        isClearable
        onChange={option => onSelected(option)}
        placeholder={placeholder ?? ""}>
    </Select>
}

export default BaseSelect;