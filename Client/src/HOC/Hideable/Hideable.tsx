import React from "react";
import "./Hideable.scss";

type State = {
    isVisible: boolean
}

export const hideableHOC = (Child: any) => {
    return class Hideable extends React.Component<any, State> {
        state = {
            isVisible: true
        }
        
        onSwitch = ({ target: { checked } }: React.ChangeEvent<HTMLInputElement>) => {
            this.setState({isVisible: checked})
        }

        render() {
            const {title} = this.props;
            const {isVisible} = this.state;
            return <div>
                <label className="checkbox-container">
                    <input className="checkbox-native" name="isVisible" checked={isVisible} onChange={this.onSwitch} type="checkbox"/>
                    <span className="checkbox-custom"></span>
                    {title}
                </label>
                {isVisible ? <Child {...this.props} />: null}
            </div>
        }        
    }
 }

export default hideableHOC;