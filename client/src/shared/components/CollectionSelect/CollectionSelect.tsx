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
