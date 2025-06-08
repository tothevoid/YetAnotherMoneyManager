import { forwardRef, Fragment, useImperativeHandle } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { useTranslation } from 'react-i18next';
import BrokerAccountSecurity from '../BrokerAccountSecurity/BrokerAccountSecurity';
import { BrokerAccountSecurityEntity } from '../../../../models/brokers/BrokerAccountSecurityEntity';
import { useBrokerAccountsSecurities } from '../../hooks/useBrokerAccountsSecurities';

interface Props {
	brokerAccountId: string
}

export interface BrokerAccountSecuritiesListRef {
	reloadData: () => Promise<void>
}

const BrokerAccountSecuritiesList = forwardRef<BrokerAccountSecuritiesListRef, Props>((props: Props, ref)=> {
	const { t } = useTranslation()

	const { 
		brokerAccountSecurities,
		updateBrokerAccountSecurityEntity,
		deleteBrokerAccountSecurityEntity,
		reloadBrokerAccountSecurities
	} = useBrokerAccountsSecurities({brokerAccountId: props.brokerAccountId});

	useImperativeHandle(ref, () => ({
		reloadData: reloadBrokerAccountSecurities,
	}));

	return (
		<Fragment>
			<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(350px, 3fr))'>
				{
					brokerAccountSecurities.map((brokerAccountSecurity: BrokerAccountSecurityEntity) => 
						<BrokerAccountSecurity onReloadBrokerAccounts={reloadBrokerAccountSecurities} 
							brokerAccountSecurity={brokerAccountSecurity} onEditCallback={updateBrokerAccountSecurityEntity} 
							onDeleteCallback={deleteBrokerAccountSecurityEntity} key={brokerAccountSecurity.id}/>)
				}
			</SimpleGrid>
		</Fragment>
	);
});

export default BrokerAccountSecuritiesList;