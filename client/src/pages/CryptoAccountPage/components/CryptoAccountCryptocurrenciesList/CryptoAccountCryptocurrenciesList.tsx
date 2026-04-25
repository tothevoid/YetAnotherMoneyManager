import React, { Fragment, useEffect, useState } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { useCryptoAccountCryptocurrencies } from '../../hooks/useCryptoAccountCryptocurrencies';
import { CryptoAccountEntity } from '../../../../models/crypto/CryptoAccountEntity';
import { CryptoAccountCryptocurrencyEntity } from '../../../../models/crypto/CryptoAccountCryptocurrencyEntity';
import CryptoAccountCryptocurrency from '../CryptoAccountCryptocurrency/CryptoAccountCryptocurrency';
import { Flex } from '@chakra-ui/react';
import CryptoAccountCryptocurrencyModal from '../../modals/CryptoAccountCryptocurrencyModal';
import { useEntityModal } from '../../../../shared/hooks/useEntityModal';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { ActiveEntityMode } from '../../../../shared/enums/activeEntityMode';
import AddButton from '../../../../shared/components/AddButton/AddButton';
import { useTranslation } from 'react-i18next';

interface Props {
	cryptoAccount: CryptoAccountEntity
}

const CryptoAccountCryptocurrenciesList: React.FC<Props> = (props: Props)=> {
	
	const { 
		activeEntity,
		modalRef,
		confirmModalRef,
		onAddClicked,
		onEditClicked,
		onDeleteClicked,
		mode,
		onActionEnded
	} = useEntityModal<CryptoAccountCryptocurrencyEntity>();
	
	const { 
		cryptoAccountCryptocurrencies,
		addCryptoAccountCryptocurrencyEntity,
		updateCryptoAccountCryptocurrencyEntity,
		deleteCryptoAccountCryptocurrencyEntity,
		reloadCryptoAccountCryptocurrencies
	} = useCryptoAccountCryptocurrencies({cryptoAccountId: props.cryptoAccount.id});

	const [context, setContext] = useState<CryptoAccountCryptocurrencyEntity>();

	useEffect(() => {
		if (activeEntity) {
			setContext(activeEntity);
		} else {
			// setContext({
			// 	cryptoAccount: props.cryptoAccount,
			// 	cryptocurrency: {},
			// 	quantity: 0
			// })
		}
	}, [activeEntity, props.cryptoAccount]);

	const onCryptocurrencyCryptoaccountSaved = async (cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity) => {
		if (mode === ActiveEntityMode.Add) {
			await addCryptoAccountCryptocurrencyEntity(cryptoAccountCryptocurrency)
		} else if (mode === ActiveEntityMode.Edit) {
			await updateCryptoAccountCryptocurrencyEntity(cryptoAccountCryptocurrency)
		}
		onActionEnded();
	}

	const { t } = useTranslation();

	const onDeleteConfirmed = async () => {
		if (!activeEntity) {
			throw new Error("Deleted entity is not set")
		}

		await deleteCryptoAccountCryptocurrencyEntity(activeEntity.id);
		onActionEnded();
	}

	return (
		<Fragment>
			<Flex justifyContent="end" mb={4}>
				<AddButton onClick={onAddClicked} buttonTitle={t('add_crypto_account_cryptocurrency_title')}/>
			</Flex>
			<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(350px, 3fr))'>
				{
					cryptoAccountCryptocurrencies.map((cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity) => 
						<CryptoAccountCryptocurrency
							onReloadCryptoAccountCryptocurrencies={reloadCryptoAccountCryptocurrencies}
							cryptoAccountCryptocurrency={cryptoAccountCryptocurrency}
							key={cryptoAccountCryptocurrency.id}
							onEditClicked={onEditClicked}
							onDeleteClicked={onDeleteClicked}
						/>)
				}
			</SimpleGrid>

			<CryptoAccountCryptocurrencyModal
				modalRef={modalRef}
				cryptoAccountCryptocurrency={context}
				onSaved={onCryptocurrencyCryptoaccountSaved}
			/>
			<ConfirmModal onConfirmed={onDeleteConfirmed}
				title={t("crypto_account_cryptocurrency_delete_title")}
				message={t("modals_delete_message")}
				confirmActionName={t("modals_delete_button")}
				ref={confirmModalRef}/>
		</Fragment>
	);
};

export default CryptoAccountCryptocurrenciesList;