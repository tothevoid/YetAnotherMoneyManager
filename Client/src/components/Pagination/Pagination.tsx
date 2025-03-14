import { Component, Fragment } from "react"
import "./Pagination.css"
import {getMonthByIndex} from "../../utils/DateUtils"
import { Calendar } from "../Calendar/Calendar"
import CalendarIcon from "../../assets/calendar.svg"
import { MdChevronLeft, MdChevronRight } from "react-icons/md"

type State = {
    isCalendarVisible: boolean
}

type Props = {
    month: number,
    year: number,
    onPageSwitched: (month: number, year: number) => void
}

class Pagination extends Component<Props, State>{

    state = {
        isCalendarVisible: false
    };

    componentDidMount = () => {
        const {month, year} = this.props;
        this.props.onPageSwitched(month, year);
    }

    pageSwitchClick = (direction: number) => () => {
        let {month, year} = this.props;
        if (direction === -1 && month === 1){
            month = 12;
            year += direction;
        } else if (direction === 1 && month === 12){
            month = 1;
            year += direction;
        } else {
            month += direction;
        }
        this.props.onPageSwitched(month, year);
    }

    onSwitchCalendarVisibility = () =>{
        const {isCalendarVisible} = this.state;
        this.setState({isCalendarVisible: !isCalendarVisible});
    }

    render(){
        const {isCalendarVisible} = this.state;
        const {month, year, onPageSwitched} = this.props;
        const date = `${getMonthByIndex(month)}'${year.toString().substring(2)}`
        return <Fragment>
        <div className="pagination-container">
                <button onClick={this.pageSwitchClick(-1)} className="paging-element paging-button page-previous"><MdChevronLeft/></button>
                <div className="current-month paging-element">
                    {date}
                    <img alt="calendar" className="calendar-icon" onClick={()=>this.onSwitchCalendarVisibility()} src={CalendarIcon}></img>
                </div>
                
                <button onClick={this.pageSwitchClick(1)} className="paging-element paging-button page-next"><MdChevronRight/></button>
            </div>
            {
                (isCalendarVisible) ?
                    <Calendar month={month} year={year} onPageSwitched={onPageSwitched}></Calendar> :
                    null 
            }
        </Fragment> 
    }
}

export default Pagination;