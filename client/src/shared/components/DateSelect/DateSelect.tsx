import "./DateSelect.scss"

import { Input } from "@chakra-ui/react";
import DatePicker from "react-datepicker";
import { Control, Controller } from "react-hook-form"

interface Props {
    name: string
    control: Control<any>
    fullWidth?: boolean,
    isDateTime?: boolean,
}

const DateSelect: React.FC<Props> = ({name, control, fullWidth = true, isDateTime = false}) => {
    const format = isDateTime ?
        "dd.MM.yyyy HH:mm:ss":
        "dd.MM.yyyy";

    return <Controller
        name={name}
        control={control}
        render={({ field: {onChange, value} }) => (
            <DatePicker
                showTimeSelect={isDateTime} 
                wrapperClassName={fullWidth ? "date-select-full-with": undefined}
                selected={value}
                onChange={onChange}
                dateFormat={format}
                customInput={<Input/>}/>
        )}
    />
}

export default DateSelect;