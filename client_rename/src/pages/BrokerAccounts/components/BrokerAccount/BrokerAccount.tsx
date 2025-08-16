import { Button, Card, Flex, Icon, Link, Span, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { BrokerAccountEntity } from '../../../../models/brokers/BrokerAccountEntity';
import { calculateDiff } from '../../../../shared/utilities/numericDiffsUtilities';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import BrokerAccountModal from '../../modals/BrokerAccountModal/BrokerAccountModal';

type Props = {
	brokerAccount: BrokerAccountEntity,
	onDeleteCallback: (account: BrokerAccountEntity) => void,
	onEditCallback: (account: BrokerAccountEntity) => void,
	onReloadBrokerAccounts: () => void
}

const BrokerAccount = (props: Props) => {
	const {id, name, broker, currency, type, initialValue, currentValue} = props.brokerAccount;

	const confirmModalRef = useRef<BaseModalRef>(null);
	const editModalRef = useRef<BaseModalRef>(null);

	const onEditClicked = () => {
		editModalRef.current?.openModal()
	};

	const onDeleteClicked = () => {
		confirmModalRef.current?.openModal()
	};

	const { t } = useTranslation();

	const accountLink = `../broker_account/${id}`;

	const { profitAndLoss, profitAndLossPercentage, color } = calculateDiff(currentValue, initialValue);

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
							<Span color={color}>({profitAndLoss.toFixed(2)} | {profitAndLossPercentage.toFixed(2)})</Span>
						</Stack>
					</Stack>
					<Flex gap={1}>
						<Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={onEditClicked}>
							<Icon color="card_action_icon_primary">
								<MdEdit/>
							</Icon>
						</Button>
						<Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={onDeleteClicked}>
							<Icon color="card_action_icon_danger">
								<MdDelete/>
							</Icon>
						</Button>
					</Flex>
				</Flex>
			</Card.Body>
		</Card.Root>
		<ConfirmModal onConfirmed={() => props.onDeleteCallback(props.brokerAccount)}
			title={t("broker_account_delete_title")}
			message={t("modals_delete_message")}
			confirmActionName={t("modals_delete_button")}
			ref={confirmModalRef}/>
		<BrokerAccountModal brokerAccount={props.brokerAccount} modalRef={editModalRef} onSaved={props.onEditCallback}/>
	</Fragment>
};

export default BrokerAccount;