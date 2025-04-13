import { MdAdd } from "react-icons/md"
import { Button, Icon } from "@chakra-ui/react"
import { useRef } from "react"

import { Fragment } from "react/jsx-runtime"
import { AccountEntity } from "../../models/accounts/AccountEntity"
import AccountModal, { AccountModalRef } from "../../modals/AccountModal/AccountModal"
import { createAccount } from "../../api/accounts/accountApi"
import { useTranslation } from "react-i18next"

type AccountProps = {
	onAdded: (account: AccountEntity) => void;
};

const AddAccountButton: React.FC<AccountProps> = ({ onAdded }) => {
	const modalRef = useRef<AccountModalRef>(null);
	
	const onAdd = () => {
		modalRef.current?.openModal()
	};

	const onAccountAdded = async (account: AccountEntity) => {
		const createdAccountId = await createAccount(account);
		if (!createdAccountId) {
			return;
		}

		account.id = createdAccountId;
		onAdded(account);
	};

	const { t } = useTranslation();

	return (
		<Fragment>
			<Button background="purple.600" onClick={onAdd}>
				<Icon size='md'>
					<MdAdd/>
				</Icon>
				{t("accounts_page_summary_add")}
			</Button>
			<AccountModal ref={modalRef} onSaved={onAccountAdded}></AccountModal>
		</Fragment>
	)
}

export default AddAccountButton