import { AddIcon } from "@chakra-ui/icons"
import { Button } from "@chakra-ui/react"
import { useRef } from "react"

import { Fragment } from "react/jsx-runtime"
import { FundEntity } from "../../models/FundEntity"
import FundModal, { FundModalRef } from "../../modals/FundModal/FundModal"

type FundProps = {
	onAdded: (fund: FundEntity) => void;
};

const AddFundButton: React.FC<FundProps> = ({ onAdded }) => {
	const modalRef = useRef<FundModalRef>(null);
	
	const onAdd = () => {
		modalRef.current?.openModal()
	};

	const onFundAdded = (fund: FundEntity) => {
		onAdded({id: "", name: fund.name, balance: Number(fund.balance)});
	};

	return (
		<Fragment>
			<Button onClick={onAdd} leftIcon={<AddIcon/>} colorScheme='purple' size='md'>
				Add fund
			</Button>
			<FundModal ref={modalRef} onSaved={onFundAdded}></FundModal>
		</Fragment>
	)
}

export default AddFundButton