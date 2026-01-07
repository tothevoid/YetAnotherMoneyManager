import { forwardRef, Fragment, useImperativeHandle } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { useTranslation } from 'react-i18next';
import BrokerAccountSecurity from '../BrokerAccountSecurity/BrokerAccountSecurity';
import { BrokerAccountSecurityEntity } from '../../../../models/brokers/BrokerAccountSecurityEntity';
import { useBrokerAccountsSecurities } from '../../hooks/useBrokerAccountsSecurities';
import { Card, Flex, Stack, Text } from '@chakra-ui/react';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { BsCurrencyExchange } from "react-icons/bs";
import { Nullable } from '../../../../shared/utilities/nullable';

interface Props {
	mainCurrencyAmount: number,
	mainCurrencyName: string,
	brokerAccountId?: Nullable<string>
}

export interface BrokerAccountSecuritiesListRef {
	reloadData: () => Promise<void>
}

const BrokerAccountSecuritiesList = forwardRef<BrokerAccountSecuritiesListRef, Props>(({ brokerAccountId, mainCurrencyAmount, mainCurrencyName }, ref)=> {
	const { t } = useTranslation()

	const { 
		brokerAccountSecurities,
		updateBrokerAccountSecurityEntity,
		deleteBrokerAccountSecurityEntity,
		reloadBrokerAccountSecurities
	} = useBrokerAccountsSecurities({brokerAccountId: brokerAccountId});

	useImperativeHandle(ref, () => ({
		reloadData: reloadBrokerAccountSecurities,
	}));

	return (
		<Fragment>
			<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(350px, 3fr))'>
				{
					<Card.Root backgroundColor="background_primary" borderColor="border_primary" >
						<Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
							<Flex justifyContent="space-between" alignItems="center">
								<Stack>
									<Stack alignItems={"center"} justifyContent="start" direction={"row"}>
										<BsCurrencyExchange size={32}/>
										<Text color="text_primary" fontSize="xl" fontWeight={900}>{mainCurrencyName}</Text>
									</Stack>
									<Text fontWeight={600}>{t("broker_account_security_card_security_quantity")}: {formatMoneyByCurrencyCulture(mainCurrencyAmount, mainCurrencyName)}</Text>
								</Stack>
							</Flex>
						</Card.Body>
					</Card.Root>
				}
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