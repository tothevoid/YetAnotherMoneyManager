import { Button, Card, Flex, Icon, Link, Span, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit, MdAttachMoney } from "react-icons/md";
import { Fragment } from 'react';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { BrokerAccountEntity } from '../../../../models/brokers/BrokerAccountEntity';
import { calculateDiff } from '../../../../shared/utilities/numericDiffsUtilities';

interface Props {
	brokerAccount: BrokerAccountEntity
	onTopUpClick: (account: BrokerAccountEntity) => void
	onEditClick: (account: BrokerAccountEntity) => void
	onDeleteClick: (account: BrokerAccountEntity) => void
}

const BrokerAccount = (props: Props) => {
	const { id, name, broker, currency, type, initialValue, currentValue } = props.brokerAccount;

	const accountLink = `../broker_account/${id}`;
	const { profitAndLoss, profitAndLossPercentage, color } = calculateDiff(currentValue, initialValue, currency.name);

	return <Fragment>
		<Card.Root backgroundColor="background_primary" borderColor="border_primary" >
			<Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
				<Flex justifyContent="space-between" alignItems="center">
					<Stack>
						<Link fontSize="2xl" fontWeight={900} color="text_primary" href={accountLink}>{name}</Link>
						<Text fontWeight={600}>{broker.name}</Text>
						<Text fontWeight={600}>{type.name}</Text>
						<Stack gapX={1} direction="row">
							<Span>{formatMoneyByCurrencyCulture(currentValue, currency.name)}</Span>
							<Span color={color}>({profitAndLoss} | {profitAndLossPercentage})</Span>
						</Stack>
					</Stack>
					<Flex gap={1}>
						<Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onTopUpClick(props.brokerAccount)}>
							<Icon color="card_action_icon_primary">
								<MdAttachMoney />
							</Icon>
						</Button>
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