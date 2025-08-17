import { Fragment, useRef } from "react";
import { DepositEntity } from "../../models/deposits/DepositEntity";
import { Button, Text, Flex, SimpleGrid, Checkbox, Box} from "@chakra-ui/react";
import DepositStats from "./components/DepositStats/DepositStats";
import Deposit from "./components/Deposit/Deposit";
import { MdAdd } from "react-icons/md";
import DepositsRangeSlider from "./components/DepositsRangeSlider/DepositsRangeSlider";
import { useTranslation } from "react-i18next";
import { BaseModalRef } from "../../shared/utilities/modalUtilities";
import DepositModal from "./modals/DepositModal/DepositModal";
import { useDeposits } from "./hooks/useDeposits";
import Placeholder from "../../shared/components/Placeholder/Placeholder";

const DepositsPage: React.FC = () => {
	const { t } = useTranslation();

	const {
		deposits,
		createDepositEntity,
		updateDepositEntity,
		deleteDepositEntity,
		depositsQueryParameters, 
		setDepositsQueryParameters
	} = useDeposits({selectedMinMonths: 0, selectedMaxMonths: 0, onlyActive: true});

	const modalRef = useRef<BaseModalRef>(null);

	const showDepositModal = () => {
		modalRef.current?.openModal()
	};

	const getAddButton = () => {
		return <Button onClick={showDepositModal} background='purple.600' size='md'>
			<MdAdd/>
			{t("deposits_list_add_button")}
		</Button>
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
						onUpdated={updateDepositEntity} 
						onCloned={createDepositEntity} 
						onDeleted={deleteDepositEntity}/>
				)
			}
		</SimpleGrid>
	</Box>
}

export default DepositsPage;