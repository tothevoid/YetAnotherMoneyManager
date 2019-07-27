import React from 'react';
import Fund from '../Fund/Fund'
import './FundsBar.css'
import FundContainer from '../FundContainer/FundContainer';
import AddFundButton from '../AddFundButton/AddFundButton';

class FundsBar extends React.Component{
    state = {
        funds: []
    }
    
    componentDidMount() {
        const funds = Array(3).fill(1).map(()=>{return {name: "abc", funds: 10}});
        this.setState({funds})
        // const API_URL = "https://localhost:44319/Fund";
        // fetch(API_URL, {method: 'GET'})
        //     .then(res => res.json())
        //     .then(res => this.setState({transactions: res}));
    }

    getCreateNewButton(){
        const SubContainer = FundContainer(true)(AddFundButton)
        return <SubContainer></SubContainer>
    }

    render(){
        const {funds} = this.state;
        return (
            <div className="funds-bar">
                {
                funds.map((transaction: any, i: number) => {
                    const SubContainer = FundContainer()(Fund)
                    return <SubContainer {...transaction} key={i}></SubContainer>
                })
                }
                {this.getCreateNewButton()}
            </div>
        );
    }
}

export default FundsBar;