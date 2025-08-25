import { Fragment } from "react";
import { DepositEntity } from "../../models/deposits/DepositEntity";
import { Flex, SimpleGrid, Checkbox, Box} from "@chakra-ui/react";
import DepositStats from "./components/DepositStats/DepositStats";
import Deposit from "./components/Deposit/Deposit";
import DepositsRangeSlider from "./components/DepositsRangeSlider/DepositsRangeSlider";
import { useTranslation } from "react-i18next";
import DepositModal from "./modals/DepositModal/DepositModal";
import { useDeposits } from "./hooks/useDeposits";
import Placeholder from "../../shared/components/Placeholder/Placeholder";
import { useEntityModal } from "../../shared/hooks/useEntityModal";
import { ConfirmModal } from "../../shared/modals/ConfirmModal/ConfirmModal";
import AddButton from "../../shared/components/AddButton/AddButton";
import { ActiveEntityMode } from "../../shared/enums/activeEntityMode";

const DepositsPage: React.FC = () => {
	const { t } = useTranslation();

	const { 
		activeEntity,
		modalRef,
		confirmModalRef,
		onAddClicked,
		onEditClicked,
		onDeleteClicked,
		mode,
		onActionEnded
	} = useEntityModal<DepositEntity>();

	const {
		deposits,
		createDepositEntity,
		updateDepositEntity,
		deleteDepositEntity,
		depositsQueryParameters, 
		setDepositsQueryParameters
	} = useDeposits({selectedMinMonths: 0, selectedMaxMonths: 0, onlyActive: true});

	const getAddButton = () => {
		return <AddButton onClick={onAddClicked} buttonTitle={t("deposits_list_add_button")}></AddButton>
	}

	const getAddButtonWithDeposits = () => {
		return getAddButton();
	}

	const getAddButtonWithoutDeposits = () => {
		return <Placeholder text={t("deposits_page_no_deposits")}>
			{getAddButton()}
		</Placeholder>
	}

	const onDepositsRangeChanged = async (fromMonths: number, toMonths: number) => {
		setDepositsQueryParameters({selectedMinMonths: fromMonths, selectedMaxMonths: toMonths, onlyActive: depositsQueryParameters.onlyActive})
	}

	const onCheckboxChanged = async (checked: boolean) => {
		setDepositsQueryParameters({...depositsQueryParameters, onlyActive: checked})
	}

	const { selectedMinMonths, selectedMaxMonths, onlyActive } = depositsQueryParameters;

	const onDepositSaved = async (deposit: DepositEntity) => {
		if (mode === ActiveEntityMode.Add) {
			await createDepositEntity(deposit)
		} else if (mode === ActiveEntityMode.Edit) {
			await updateDepositEntity(deposit)
		}
		onActionEnded();
	}

	const onDeleteConfirmed = async () => {
		if (!activeEntity) {
            throw new Error("Deleted entity is not set")
        }

        await deleteDepositEntity(activeEntity);
		onActionEnded();
    }

	const onCloneClicked = async (deposit: DepositEntity) => {
        await createDepositEntity(deposit);
    }

	return <Box paddingTop={5}>
		{
			deposits.length > 0 && selectedMaxMonths && selectedMaxMonths ?
				<DepositStats onlyActive={onlyActive} selectedMinMonths={selectedMinMonths} selectedMaxMonths={selectedMaxMonths}/>:
				<Fragment/>
		}
		<DepositsRangeSlider onDepositsRangeChanged={onDepositsRangeChanged} />
		{
			deposits.length > 0 ?
				<Flex gap={4} direction="row" alignItems="center" paddingTop={4}>
					{getAddButtonWithDeposits()}
					<Checkbox.Root checked={onlyActive} onCheckedChange={(details) => onCheckboxChanged(!!details.checked)} variant="solid">
						<Checkbox.HiddenInput />
						<Checkbox.Control />
						<Checkbox.Label color="text_primary">{t("deposits_list_only_active")}</Checkbox.Label>
					</Checkbox.Root>
				</Flex>:
				getAddButtonWithoutDeposits()
		}
		<DepositModal modalRef={modalRef} onSaved={createDepositEntity}/>
		<SimpleGrid pt={5} pb={5} gap={6} templateColumns='repeat(auto-fill, minmax(300px, 4fr))'>
			{
				deposits.map((deposit: DepositEntity) => 
					<Deposit key={deposit.id} deposit={deposit} 
						onEditClicked={onEditClicked} 
						onCloneClicked={onCloneClicked} 
						onDeleteClicked={onDeleteClicked}/>
				)
			}
		</SimpleGrid>
		<ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("deposit_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <DepositModal deposit={activeEntity} modalRef={modalRef} onSaved={onDepositSaved}/>
	</Box>
}

export default DepositsPage;