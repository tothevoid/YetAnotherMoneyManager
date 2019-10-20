import React, { Component } from "react"
import "./Calendar.scss"
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
            Array.from({length: 12}, (element: number, index:number) => 
                (minimalYear + index).toString()) :
            getMonthsNames().map((name: string) => name.substring(0, 3));
    }

    onElementClick = (value: string) => {
        const {onPageSwitched} = this.props;
        const {isYearsMode} = this.state;
        if (isYearsMode){
            const year = parseInt(value);
            onPageSwitched(this.props.month, year);
            this.setState({isYearsMode: false, title: this.getTtile(false, year)});
        } else {
            const monthIndex = this.getMonthIndex(value);
            onPageSwitched(monthIndex, this.props.year);
        }
    }

    getMonthIndex = (value: string): number =>{
        const months = getMonthsNames();
        const month = months
            .filter(x => x.toLowerCase().startsWith(value.toLowerCase()));
        return months.indexOf(month[0]) + 1
    }

    getClasses = (value: string): string =>{
        const defaultClass = "calendar-element";
        const {year, month} = this.props;
        const {isYearsMode} = this.state;
        const monthIndex = this.getMonthIndex(value);
        return ((isYearsMode && year === parseInt(value)) || 
            (!isYearsMode && month === monthIndex)) ?
            `${defaultClass} current` :
            defaultClass;
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
                <button onClick={() => this.onArrowClick(1)} disabled={!isYearsMode} className="next-button">&#8594;</button>
            </div>
            
            <div className="calendar-content">
                {this.createArray().map((element, index)=>{
                    return <div key={index} onClick={()=>this.onElementClick(element)}
                        className={this.getClasses(element)}>{element}</div>
                })}
            </div>
        </div>
    }  
}