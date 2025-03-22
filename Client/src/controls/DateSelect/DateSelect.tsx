import "./DateSelect.scss"

import { Input } from "@chakra-ui/react";
import DatePicker from "react-datepicker";
import { Control, Controller } from "react-hook-form"

interface Props {
    name: string
    control: Control<any>
    fullWidth?: boolean
}

const DateSelect: React.FC<Props> = ({name, control, fullWidth = true}) => {
    return <Controller
        name={name}
        control={control}
        render={({ field: {onChange, value} }) => (
            <DatePicker
                wrapperClassName={fullWidth ? "date-select-full-with": undefined}
                selected={value}
                onChange={onChange}
                dateFormat="dd.MM.yyyy"
                customInput={<Input/>}/>
        )}
    />
}

export default DateSelect;