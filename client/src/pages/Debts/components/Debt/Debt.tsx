import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { useTranslation } from 'react-i18next';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { DebtEntity } from '../../../../models/debts/DebtEntity';
import { formatDate } from '../../../../shared/utilities/formatters/dateFormatter';

type Props = {
	debt: DebtEntity,
	onEditClicked: (debt: DebtEntity) => void,
	onDeleteClicked: (debt: DebtEntity) => void,
}

const Debt = (props: Props) => {
	const { name, amount, date, currency } = props.debt;
	const { i18n } = useTranslation();

	return <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
		<Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
			<Flex justifyContent="space-between" alignItems="center">
				<Stack>
					<Text fontWeight={600}>{name}</Text>
					<Text fontWeight={600}>{formatMoneyByCurrencyCulture(amount, currency.name)}</Text>
					<Text fontWeight={600}>{formatDate(date, i18n)}</Text>
				</Stack>
				<Flex gap={1}>
					<Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onEditClicked(props.debt)}>
						<Icon color="card_action_icon_primary">
							<MdEdit/>
						</Icon>
					</Button>
					<Button borderColor="background_secondary" background="button_background_secondary" size={'sm'}  onClick={() => props.onDeleteClicked(props.debt)}>
						<Icon color="card_action_icon_danger">
							<MdDelete/>
						</Icon>
					</Button>
				</Flex>
			</Flex>
		</Card.Body>
	</Card.Root>
};

export default Debt;