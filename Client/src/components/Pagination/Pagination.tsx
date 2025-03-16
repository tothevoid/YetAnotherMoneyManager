import { Fragment, useEffect, useState } from "react"
import "./Pagination.css"
import {getMonthByIndex} from "../../utils/DateUtils"
import { Calendar } from "../Calendar/Calendar"
import CalendarIcon from "../../assets/calendar.svg"
import { MdChevronLeft, MdChevronRight } from "react-icons/md"
import { useTranslation } from "react-i18next"

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
    <div className="pagination-container">
            <button onClick={pageSwitchClick(-1)} className="paging-element paging-button page-previous"><MdChevronLeft/></button>
            <div className="current-month paging-element">
                {date}
                <img alt="calendar" className="calendar-icon" onClick={() => onSwitchCalendarVisibility()} src={CalendarIcon}></img>
            </div>
            
            <button onClick={pageSwitchClick(1)} className="paging-element paging-button page-next"><MdChevronRight/></button>
        </div>
        {
            (isCalendarVisible) ?
                <Calendar month={month} year={year} onPageSwitched={onPageSwitched}></Calendar> :
                null 
        }
    </Fragment> 
    
}

export default Pagination;