import { MdAdd } from "react-icons/md"
import { Button, Icon } from "@chakra-ui/react"
import { useRef } from "react"

import { Fragment } from "react/jsx-runtime"
import { FundEntity } from "../../models/FundEntity"
import FundModal, { FundModalRef } from "../../modals/FundModal/FundModal"
import { createFund } from "../../api/fundApi"
import { useTranslation } from "react-i18next"

type FundProps = {
	onAdded: (fund: FundEntity) => void;
};

const AddFundButton: React.FC<FundProps> = ({ onAdded }) => {
	const modalRef = useRef<FundModalRef>(null);
	
	const onAdd = () => {
		modalRef.current?.openModal()
	};

	const onFundAdded = async (fund: FundEntity) => {
		const newFund = {id: "", name: fund.name, balance: Number(fund.balance)};
		const createdFundId = await createFund(newFund);
		if (!createdFundId) {
			return;
		}

		newFund.id = createdFundId;
		onAdded(newFund);
	};

	const { t } = useTranslation();

	return (
		<Fragment>
			<Button background="purple.600" onClick={onAdd}>
				<Icon size='md'>
					<MdAdd/>
				</Icon>
				{t("manager_funds_summary_add")}
			</Button>
			<FundModal ref={modalRef} onSaved={onFundAdded}></FundModal>
		</Fragment>
	)
}

export default AddFundButton