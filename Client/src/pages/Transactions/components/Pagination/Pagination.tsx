import { Fragment, useEffect, useState } from "react"
import "./Pagination.css"
import {getMonthByIndex} from "../../../../shared/utilities/dateUtils"
import { Calendar } from "../../../../shared/components/Calendar/Calendar"
import { MdChevronLeft, MdChevronRight } from "react-icons/md"
import { useTranslation } from "react-i18next"
import { Button, Flex } from "@chakra-ui/react"
import { MdCalendarMonth } from "react-icons/md";

type State = {
    isCalendarVisible: boolean
}

type Props = {
    month: number,
    year: number,
    onPageSwitched: (month: number, year: number) => void
}

const Pagination: React.FC<Props> = (props: Props) => {
    const [state, setState] = useState<State>({isCalendarVisible: false})

    useEffect(() => {
        const {month, year} = props;
        props.onPageSwitched(month, year);
    }, []);
    

    const pageSwitchClick = (direction: number) => () => {
        let {month, year} = props;
        if (direction === -1 && month === 1){
            month = 12;
            year += direction;
        } else if (direction === 1 && month === 12){
            month = 1;
            year += direction;
        } else {
            month += direction;
        }
        props.onPageSwitched(month, year);
    }

    const onSwitchCalendarVisibility = () =>{
        const {isCalendarVisible} = state;
        setState({isCalendarVisible: !isCalendarVisible});
    }

    const { i18n } = useTranslation();
    const {isCalendarVisible} = state;
    const {month, year, onPageSwitched} = props;
    const date = `${getMonthByIndex(month, i18n)}'${year.toString().substring(2)}`
    return <Fragment>
        <Flex justifyContent="center" className="pagination-container" color="text_primary">
            <Button color="text_primary" background={'background_primary'} onClick={pageSwitchClick(-1)} className="paging-element paging-button page-previous"><MdChevronLeft/></Button>
            <Button minW={125} color="text_primary" background={'background_primary'} borderRadius={0}  className="calendar-icon current-month paging-element" 
                onClick={onSwitchCalendarVisibility}>
                {date}
                <MdCalendarMonth/>
            </Button>
            <Button color="text_primary" background={'background_primary'} onClick={pageSwitchClick(1)} className="paging-element paging-button page-next"><MdChevronRight/></Button>
        </Flex>
        {
            (isCalendarVisible) ?
                <Calendar month={month} year={year} onPageSwitched={onPageSwitched}></Calendar> :
                null 
        }
    </Fragment> 
    
}

export default Pagination;