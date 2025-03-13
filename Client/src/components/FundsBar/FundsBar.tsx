import React, { Fragment } from 'react';
import Fund from '../Fund/Fund'
import { FundEntity } from '../../models/FundEntity';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex, Text } from '@chakra-ui/react';
import AddFundButton from '../AddFundButton/AddFundButton';
import { formatMoney } from '../../formatters/moneyFormatter';

const calculateTotal = (items: FundEntity[]) => {
	return items.reduce((total: number, item: FundEntity) => total += item.balance, 0);
};

type Props = {
	funds: FundEntity[],
	onAddFundCallback: (fund: FundEntity) => void,
	onDeleteFundCallback: (fund: FundEntity) => void,
	onUpdateFundCallback: (fund: FundEntity) => void
}

const FundsBar: React.FC<Props> = (props) => {
	// TODO: Check unnecessary calculations
	const total = calculateTotal(props.funds);

	const {onAddFundCallback, onUpdateFundCallback, onDeleteFundCallback } = props;
 
	return (
		<Fragment>
			<Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
				<Text fontSize='3xl'>Funds: {formatMoney(total)}</Text>
				<AddFundButton onAdded={onAddFundCallback}></AddFundButton>
			</Flex>
			<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(300px, 3fr))'>
				{
				props.funds.map((fund: FundEntity) => {
					return <Fund fund={fund} onEditCallback={onUpdateFundCallback} 
						onDeleteCallback={onDeleteFundCallback} key={fund.id}></Fund>
				})
				}
			</SimpleGrid>
		</Fragment>
	);
}

export default FundsBar;