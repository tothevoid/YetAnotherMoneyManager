import { Box, Button, Icon, Table, Text } from "@chakra-ui/react";
import { useRef, useState } from "react";
import { MdAdd, MdDelete, MdEdit } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { CryptoProviderEntity } from "../../../../models/crypto/CryptoProviderEntity";
import CryptoProviderModal from "../../modals/CryptoProviderModal/CryptoProviderModal";
import { useCryptoProviders } from "../../hooks/useCryptoProviders";

interface Props {}

const CryptoProvidersTable: React.FC<Props> = () => {
	const { t } = useTranslation();
	const modalRef = useRef<BaseModalRef>(null);
	const confirmModalRef = useRef<BaseModalRef>(null);
	const [selectedCryptoProvider, setSelectedCryptoProvider] = useState<CryptoProviderEntity | null>();

	const {
		cryptoProviders,
		createCryptoProviderEntity,
		updateCryptoProviderEntity,
		deleteCryptoProviderEntity
	} = useCryptoProviders();
 
	const onAdd = () => {
		modalRef.current?.openModal()
	};

	const onCryptoProviderSaved = (cryptoProvider: CryptoProviderEntity) => {
		if (selectedCryptoProvider?.id === cryptoProvider.id) {
			updateCryptoProviderEntity(cryptoProvider);
		} else {
			createCryptoProviderEntity(cryptoProvider);
		}
		setSelectedCryptoProvider(null);
	}

	const onEditClicked = (cryptoProvider: CryptoProviderEntity) => {
		setSelectedCryptoProvider(cryptoProvider);

		modalRef.current?.openModal()
	}

	const onDeleteClicked = (cryptoProvider: CryptoProviderEntity) => {
		setSelectedCryptoProvider(cryptoProvider);
		confirmModalRef.current?.openModal()
	}

	const onCryptoProviderDeleteConfirmed = () => {
		if (!selectedCryptoProvider) {
			return;
		}

		deleteCryptoProviderEntity(selectedCryptoProvider);
		setSelectedCryptoProvider(null);
	}

	return <Box color="text_primary">
		<Box>
			<Button background="purple.600" onClick={onAdd}>
				<Icon size='md'>
					<MdAdd/>
				</Icon>
				{t("transaction_type_data_add")}
			</Button>
		</Box>
		<Table.Root>
			<Table.Header>
				<Table.Row border="none" bg="none" color="text_primary">
					<Table.ColumnHeader color="text_primary">Name</Table.ColumnHeader>
					<Table.ColumnHeader/>
					<Table.ColumnHeader/>
				</Table.Row>
			</Table.Header>
			<Table.Body>
				{
					cryptoProviders.map((cryptoProvider: CryptoProviderEntity) => {
						return <Table.Row border="none" bg="none" color="text_primary" key={cryptoProvider.id}>
							<Table.Cell>
								<Text>
									{cryptoProvider.name}
								</Text>
							</Table.Cell>
							<Table.Cell width={10}>
								<Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => onEditClicked(cryptoProvider)}>
									<Icon color="card_action_icon_primary">
										<MdEdit/>
									</Icon>
								</Button>
							</Table.Cell>
							<Table.Cell width={10}>
								<Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => onDeleteClicked(cryptoProvider)}>
									<Icon color="card_action_icon_danger">
										<MdDelete/>
									</Icon>
								</Button>
							</Table.Cell>
						</Table.Row>
					})
				}
			</Table.Body>
		</Table.Root>
		<CryptoProviderModal cryptoProvider={selectedCryptoProvider} modalRef={modalRef} onSaved={onCryptoProviderSaved}/>
		<ConfirmModal onConfirmed={onCryptoProviderDeleteConfirmed}
			title={t("transaction_type_delete_title")}
			message={t("modals_delete_message")}
			confirmActionName={t("modals_delete_button")}
			ref={confirmModalRef}/>
	</Box>
}

export default CryptoProvidersTable;