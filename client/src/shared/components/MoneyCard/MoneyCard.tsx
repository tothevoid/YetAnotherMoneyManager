import React from "react"
import StatsCard from "../StatsCard/StatsCard";
import { formatMoneyByCurrencyCulture } from "../../utilities/formatters/moneyFormatter";

interface Props {
    title: string,
    value: number,
    currency: string
}

const MoneyCard: React.FC<Props> = ({title, value, currency}) => {
    return <StatsCard title={title} value={formatMoneyByCurrencyCulture(value, currency)}/>
}

export default MoneyCard;