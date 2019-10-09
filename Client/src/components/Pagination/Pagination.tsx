import React, { Component } from "react"
import "./Pagination.css"
import {getMonthByIndex} from "../../utils/DateUtils"
import { Calendar } from "../Calendar/Calendar"

type Props = {
    month: number,
    year: number,
    onPageSwitched: (month: number, year: number) => void
}

class Pagination extends Component<Props, any>{
    componentDidMount = () => {
        const {month, year} = this.props;
        this.props.onPageSwitched(month, year);
    }

    pageSwitchClick = (direction: number) => (e:any) => {
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

    render(){
        const {month, year, onPageSwitched} = this.props;
        const date = `${getMonthByIndex(month)} ${year.toString().substring(2)}`
        return <div className="pagination">
            <div className="pagination-container">
                <button onClick={this.pageSwitchClick(-1)} className="paging-element paging-button page-previous">Previous</button>
                <div className="current-month paging-element">{date}</div>
                <button onClick={this.pageSwitchClick(1)} className="paging-element paging-button page-next">Next</button>
            </div>
            <Calendar month={month} year={year} onPageSwitched={onPageSwitched} isVisible={true}></Calendar>
        </div>
    }
}

export default Pagination;