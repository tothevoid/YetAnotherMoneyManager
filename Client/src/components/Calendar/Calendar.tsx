import React, { Component } from "react"
import "./Calendar.css"
import { getMonthsNames } from "../../utils/DateUtils";

type Props = {
    year: number,
    month: number,
    onPageSwitched: (month: number, year: number) => void
}

type State = {
    minimalYear: number;
    isYearsMode: boolean,
    title: string
}

export class Calendar extends Component<Props,State>{
    
    getTtile = (isYearsMode: boolean, year: number): string => {
        return (isYearsMode) ? 
            `${year} - ${year + 11}` :
            year.toString();
    }

    state = {
        minimalYear: this.props.year,
        isYearsMode: false,
        title: this.getTtile(false, this.props.year)
    }

    createArray = (): string[] => {
        const {isYearsMode, minimalYear} = this.state;
        return (isYearsMode) ?
            Array.from({length: 12}, (x:number, i:number) => 
                (minimalYear + i).toString()) :
            getMonthsNames().map((element)=> element.substring(0, 3));
    }

    onElementClick = (value: string) => {
        const {onPageSwitched} = this.props;
        const {isYearsMode} = this.state;
        if (isYearsMode){
            const val = parseInt(value);
            onPageSwitched(this.props.month, val);
            this.setState({isYearsMode: false, title: this.getTtile(false, val)});
        } else {
            const months = getMonthsNames();
            const val = months
                .filter(x => x.toLowerCase().startsWith(value.toLowerCase()));
            onPageSwitched(months.indexOf(val[0]) + 1, this.props.year);
        }
    }

    getClasses = (value: string): string =>{
        const defaultClass = "number";
        const months = getMonthsNames();
        const val = months
            .filter(x => x.toLowerCase().startsWith(value.toLowerCase()));
        const {year, month} = this.props;
        const {isYearsMode} = this.state;
        if ((isYearsMode && year === parseInt(value)) || 
            (!isYearsMode && month === months.indexOf(val[0]) + 1)){
            return `${defaultClass} current`;
        }
        return defaultClass;
    }

    onLabelClick = () => {
        const {year} = this.props;
        const {isYearsMode} = this.state;
        if (!isYearsMode) {
            this.setState({isYearsMode: true, 
                title: this.getTtile(true, year)});
        }
    }

    onArrowClick = (value: number) => {
        const {minimalYear} = this.state;
        const newMininimal = minimalYear + value * 12;
        this.setState({minimalYear: newMininimal, title: this.getTtile(true, newMininimal)});
    }

    render = () => {
        const {title, isYearsMode} = this.state;

        return <div className="calendar">
            <div className="calendar-header">
                <button onClick={() => this.onArrowClick(-1)} disabled={!isYearsMode} className="prev-button">&#8592;</button>
                <span onClick={() => this.onLabelClick()} className="calendar-title">{title}</span>
                <button onClick={() => this.onArrowClick(1)} disabled={!isYearsMode} className="prev-button">&#8594;</button>
            </div>
            
            <div className="container">
                {this.createArray().map((element, ix)=>{
                    return <div key={ix} onClick={()=>this.onElementClick(element)}
                        className={this.getClasses(element)}>{element}</div>
                })}
            </div>
        </div>
    }  
}