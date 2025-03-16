import { useState } from "react"
import "./Calendar.scss"
import { getMonthsNames } from "../../utils/DateUtils";
import { useTranslation } from "react-i18next";

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

export const Calendar: React.FC<Props> = (props: Props) => {
    const getTitle = (isYearsMode: boolean, year: number): string => {
        return (isYearsMode) ? 
            `${year} - ${year + 11}` :
            year.toString();
    }

    const getDefaultState = () => {
        return {
            minimalYear: props.year,
            isYearsMode: false,
            title: getTitle(false, props.year)
        }
    }


    const createArray = (): string[] => {
        const {isYearsMode, minimalYear} = state;
        return (isYearsMode) ?
            Array.from({length: 12}, (_, index:number) => 
                (minimalYear + index).toString()) :
            getMonthsNames(i18n).map((name: string) => name.substring(0, 3));
    }

    const onElementClick = (value: string) => {
        const {onPageSwitched} = props;
        const {isYearsMode} = state;
        if (isYearsMode){
            const year = parseInt(value);
            onPageSwitched(props.month, year);
            setState((currentState) => {return {...currentState, isYearsMode: false, title: getTitle(false, year)}});
        } else {
            const monthIndex = getMonthIndex(value);
            onPageSwitched(monthIndex, props.year);
        }
    }

    const getMonthIndex = (value: string): number =>{
        const months = getMonthsNames(i18n);
        const month = months
            .filter(x => x.toLowerCase().startsWith(value.toLowerCase()));
        return months.indexOf(month[0]) + 1
    }

    const getClasses = (value: string): string =>{
        const defaultClass = "calendar-element";
        const {year, month} = props;
        const {isYearsMode} = state;
        const monthIndex = getMonthIndex(value);
        return ((isYearsMode && year === parseInt(value)) || 
            (!isYearsMode && month === monthIndex)) ?
            `${defaultClass} current` :
            defaultClass;
    }

    const onLabelClick = () => {
        const {year} = props;
        const {isYearsMode} = state;
        if (!isYearsMode) {
            setState((currentState) => {
                return {...currentState, isYearsMode: true, title: getTitle(true, year)}
            });
        }
    }

    const onArrowClick = (value: number) => {
        const {minimalYear} = state;
        const newMininimal = minimalYear + value * 12;
        setState((currentState) => {
            return { ...currentState, minimalYear: newMininimal, title: getTitle(true, newMininimal)}
        });
    }

    const [state, setState] = useState<State>(getDefaultState)
    const {title, isYearsMode} = state;

    const { i18n} = useTranslation();

    return <div className="calendar">
        <div className="calendar-header">
            <button onClick={() => onArrowClick(-1)} disabled={!isYearsMode} className="prev-button">&#8592;</button>
            <span onClick={() => onLabelClick()} className="calendar-title">{title}</span>
            <button onClick={() => onArrowClick(1)} disabled={!isYearsMode} className="next-button">&#8594;</button>
        </div>
        
        <div className="calendar-content">
            {createArray().map((element, index)=>{
                return <div key={index} onClick={()=>onElementClick(element)}
                    className={getClasses(element)}>{element}</div>
            })}
        </div>
    </div> 
}