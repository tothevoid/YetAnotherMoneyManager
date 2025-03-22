import { Select } from "chakra-react-select";
import { Control, Controller } from "react-hook-form"

interface Props<T> {
    name: string
    placeholder?: string
    control: Control<any>
    collection: T[],
    labelSelector: (item: T) => string,
    valueSelector: (item: T) => string,
}

//TODO: Fix generics for lambdas
type CollectionSelectProps<T = any> = React.FC<Props<T>>

const CollectionSelect: CollectionSelectProps = ({name, placeholder, control, collection = [], labelSelector, valueSelector}) => {
    return <Controller
        name={name}
        control={control}
        render={({ field }) => (
            <Select
                {...field}
                getOptionLabel={labelSelector}
                getOptionValue={valueSelector}
                options={collection}
                isClearable
                placeholder={placeholder ?? ""}>
            </Select>
        )}
    />
}

export default CollectionSelect;