import { useRef } from "react";
import { SimpleGrid, Box, Checkbox, Flex} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { ClientDebtEntity } from "../../models/debts/DebtEntity";
import { ClientDebtPaymentEntity } from "../../models/debts/DebtPaymentEntity";
import Debt from "./components/Debt/Debt";
import DebtPayment from "./components/DebtPayment/DebtPayment";
import { BaseModalRef } from "../../shared/utilities/modalUtilities";
import DebtModal from "./modals/DebtModal.tsx/DebtModal";
import ShowModalButton from "../../shared/components/ShowModalButton/ShowModalButton";
import DebtPaymentModal from "./modals/DebtPaymentModal/DebtPaymentModal";
import { useDebts } from "./hooks/useDebts";
import { useDebtPayments } from "./hooks/useDebtPayments";
import SwitchButton from "../../shared/components/SwitchButton/SwitchButton";

interface Props {}

const DebtsPage: React.FC<Props> = () => {
	const { t } = useTranslation();

	const {
		debts,
		createDebtEntity,
		updateDebtEntity,
		deleteDebtEntity,
		debtQueryParameters,
		setDebtQueryParameters
	} = useDebts({onlyActive: true});

    const {
        debtPayments,
		createDebtPaymentEntity,
		updateDebtPaymentEntity,
		deleteDebtPaymentEntity,
    } = useDebtPayments();

	const modalRef = useRef<BaseModalRef>(null);
		
	const onAdd = () => {
		modalRef.current?.openModal()
	};

	const debtPaymentModalRef = useRef<BaseModalRef>(null);
	
	const onAddDebtPayment = () => {
		debtPaymentModalRef.current?.openModal()
	};

	const onOnlyActiveSwitched = (onlyActive: boolean) => {
		setDebtQueryParameters({onlyActive});
	}

	return (
		<Box paddingBlock={10}>
			<Flex justifyContent="space-between">
				<SwitchButton active={debtQueryParameters.onlyActive} title={t("debts_page_only_active")} onSwitch={onOnlyActiveSwitched}/>
				<ShowModalButton buttonTitle={t("debts_page_add_debt")} onClick={onAdd}>
					<DebtModal modalRef={modalRef} onSaved={createDebtEntity}/>
				</ShowModalButton>
			</Flex>
			<SimpleGrid pt={5} pb={5} gap={6} templateColumns='repeat(auto-fill, minmax(300px, 4fr))'>
				{
					debts.map((debt: ClientDebtEntity) => 
						<Debt key={debt.id} debt={debt} 
							onEditCallback={updateDebtEntity}
							onDeleteCallback={deleteDebtEntity}/>
					)
				}
			</SimpleGrid>
			<ShowModalButton buttonTitle={t("debts_page_add_payment")} onClick={onAddDebtPayment}>
				<DebtPaymentModal modalRef={debtPaymentModalRef} onSaved={createDebtPaymentEntity}/>
			</ShowModalButton>
			<Box>
				{
					debtPayments.map((payment: ClientDebtPaymentEntity) => 
						<DebtPayment key={payment.id} debtPayment={payment}
							onEditCallback={updateDebtPaymentEntity} 
							onDeleteCallback={deleteDebtPaymentEntity}/>
					)
				}
			</Box>
		</Box>
	)
	
}

export default DebtsPage;