import { Box, Button, Icon, Text } from "@chakra-ui/react";
import { useMemo, useRef, useState } from "react";
import { MdAdd, MdDelete, MdEdit } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { CryptoProviderEntity } from "../../../../models/crypto/CryptoProviderEntity";
import CryptoProviderModal from "../../modals/CryptoProviderModal/CryptoProviderModal";
import { useCryptoProviders } from "../../hooks/useCryptoProviders";
import DataTable, { ColumnDef } from "../../../../shared/components/DataTable/DataTable";

const CryptoProvidersTable: React.FC = () => {
	const { t } = useTranslation();
	const modalRef = useRef<BaseModalRef>(null);
	const confirmModalRef = useRef<BaseModalRef>(null);
	const [selectedCryptoProvider, setSelectedCryptoProvider] = useState<CryptoProviderEntity | null>();

	const {
		cryptoProviders,
		isCryptoProvidersLoading,
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

	const onCryptoProviderDeleteConfirmed = async () => {
		if (!selectedCryptoProvider) {
			return;
		}

		await deleteCryptoProviderEntity(selectedCryptoProvider);
		setSelectedCryptoProvider(null);
	}

	const columns: ColumnDef<CryptoProviderEntity>[] = useMemo(() => [
		{
			header: t("entity_crypto_provider_name"),
			render: (cryptoProvider) => <Text>{cryptoProvider.name}</Text>
		},
		{
			width: 10,
			render: (cryptoProvider) => (
				<Button
					borderColor="background_secondary"
					background="button_background_secondary"
					size={'sm'}
					onClick={() => onEditClicked(cryptoProvider)}
				>
					<Icon color="card_action_icon_primary">
						<MdEdit/>
					</Icon>
				</Button>
			)
		},
		{
			width: 10,
			render: (cryptoProvider) => (
				<Button
					borderColor="background_secondary"
					background="button_background_secondary"
					size={'sm'}
					onClick={() => onDeleteClicked(cryptoProvider)}
				>
					<Icon color="card_action_icon_danger">
						<MdDelete/>
					</Icon>
				</Button>
			)
		}
	], [t, cryptoProviders]);

	return <Box color="text_primary">
		<Box mb={4}>
			<Button background="action_primary" onClick={onAdd}>
				<Icon size='md'>
					<MdAdd/>
				</Icon>
				{t("entity_crypto_provider_add")}
			</Button>
		</Box>
		<DataTable
			data={cryptoProviders}
			columns={columns}
			keyExtractor={(item) => item.id}
			isLoading={isCryptoProvidersLoading}
			skeletonRows={5}
		/>
		<CryptoProviderModal cryptoProvider={selectedCryptoProvider} modalRef={modalRef} onSaved={onCryptoProviderSaved}/>
		<ConfirmModal onConfirmed={onCryptoProviderDeleteConfirmed}
			title={t("crypto_providers_delete_title")}
			message={t("modals_delete_message")}
			confirmActionName={t("modals_delete_button")}
			ref={confirmModalRef}/>
	</Box>
}

export default CryptoProvidersTable;