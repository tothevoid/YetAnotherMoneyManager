import { Button, Card, Flex, Icon, Link, Span, Stack, Text, Image } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useEffect, useState } from 'react';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { BrokerAccountEntity } from '../../../../models/brokers/BrokerAccountEntity';
import { getBankIconUrl } from '../../../../api/banks/bankApi';
import { getPortfolioValues } from '../../../../api/brokers/brokerAccountSummaryApi';
import { BrokerAccountPortfolioEntity } from '../../../../models/brokers/BrokerAccountPortfolioEntity';

interface Props {
	brokerAccount: BrokerAccountEntity
	onEditClick: (account: BrokerAccountEntity) => void
	onDeleteClick: (account: BrokerAccountEntity) => void
}

const BrokerAccount = (props: Props) => {
	const { id, name, broker, currency, type, bank } = props.brokerAccount;

	const [portfolio, setPortfolio] = useState<BrokerAccountPortfolioEntity | null>(null);

	useEffect(() => {
		const fetchPortfolioValues = async () => {
			const values = await getPortfolioValues(id)
			if (values) {
				setPortfolio(values);
			}
		}

		fetchPortfolioValues()
	}, [id]);

	const accountLink = `../broker_account/${id}`;

	if (!portfolio) {
		return <Fragment/>
	}

	const color = portfolio.currentAmount >= 0 ? "gain": "loss";

	return <Fragment>
		<Card.Root backgroundColor="background_primary" borderColor="border_primary" >
			<Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
				<Flex justifyContent="space-between" alignItems="center">
					<Stack>
						<Flex gapX={2} alignItems={"center"}>
							{bank?.iconKey && <Image fit={"contain"} h={6} w={6} rounded={4} src={getBankIconUrl(bank?.iconKey)}/>}
							<Link fontSize="2xl" fontWeight={900} color="text_primary" href={accountLink}>{name}</Link>
						</Flex>
						<Text fontWeight={600}>{broker.name}</Text>
						<Text fontWeight={600}>{type.name}</Text>
						<Stack gapX={1} direction="row">
							<Span>{formatMoneyByCurrencyCulture(portfolio?.currentAmount, currency.name)}</Span>
							<Span color={color}>({formatMoneyByCurrencyCulture(portfolio.profitAndLoss, currency.name)})</Span>
						</Stack>
					</Stack>
					<Flex gap={1}>
						<Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onEditClick(props.brokerAccount)}>
							<Icon color="card_action_icon_primary">
								<MdEdit/>
							</Icon>
						</Button>
						<Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onDeleteClick(props.brokerAccount)}>
							<Icon color="card_action_icon_danger">
								<MdDelete/>
							</Icon>
						</Button>
					</Flex>
				</Flex>
			</Card.Body>
		</Card.Root>
	</Fragment>
};

export default BrokerAccount;