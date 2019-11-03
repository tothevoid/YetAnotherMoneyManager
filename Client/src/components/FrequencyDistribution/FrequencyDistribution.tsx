import React from "react"
import "./FrequencyDistribution.scss"
import { FrequencyDistributionModel } from "./FrequencyDistributionModel";

type Props = {
    collection: FrequencyDistributionModel[]
}

const getSum = (collection: FrequencyDistributionModel[]): number => {
    return collection.reduce((total: number, freq: FrequencyDistributionModel) => total + freq.value, 0);
}

const getColor = (index: number) => {
    const colors = [
        "#D13438", "#0078D7", "#00CC6A", "#FFB900", "#EA005E", "#881798", "#F7630C"
    ]
    return (index + 1 > colors.length) ? 
        "#FFFFFF":
        colors[index];
}

const compare = (current: FrequencyDistributionModel, next: FrequencyDistributionModel) => {
    if (current.value < next.value) {
        return 1;
    } else if (current.value > next.value) {
        return -1;
    } else {
        return 0;
    }
}

const FrequencyDistribution = (props: Props) => {
    if (!props.collection || props.collection.length === 0) return null;

    const collection = props.collection.sort(compare);
    const total = getSum(collection);

    return <div className="distibution">
        <div className="distribution-header">
        {
            collection.map((freq: FrequencyDistributionModel, index: number)=>{
                const width = `${(100 / collection.length).toFixed(2)}%`;
                const background = getColor(index);
                return <div key={freq.id} className="distribution-name" style={{width}}>
                    <div style={{background}} className="indicator"></div>
                    <span className="indicator-title">{freq.name} {`(${freq.value}₽)`}</span>
                </div>
            })
        }
        </div>
        <div className="distribution-content">
            {
                collection.map((freq: FrequencyDistributionModel, index: number)=>{
                    const width = `${(freq.value / total * 100).toFixed(2)}%`;
                    const background = getColor(index);
                    return <div key={freq.id} className="frequency" title={width} style={{width}}>
                        <div className="distribution-value" style={{background}}></div>
                    </div>
                })
            }
        </div>
    </div>
}

export default FrequencyDistribution;