import React, { Fragment } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { useCryptoAccountCryptocurrencies } from '../../hooks/useCryptoAccountCryptocurrencies';
import { ClientCryptoAccountEntity } from '../../../../models/crypto/CryptoAccountEntity';
import { ClientCryptoAccountCryptocurrencyEntity } from '../../../../models/crypto/CryptoAccountCryptocurrencyEntity';
import CryptoAccountCryptocurrency from '../CryptoAccountCryptocurrency/CryptoAccountCryptocurrency';

interface Props {
	cryptoAccount: ClientCryptoAccountEntity
}

const CryptoAccountCryptocurrenciesList: React.FC<Props> = (props: Props)=> {
	const { 
        cryptoAccountCryptocurrencies,
        reloadCryptoAccountCryptocurrencies
	} = useCryptoAccountCryptocurrencies({cryptoAccountId: props.cryptoAccount.id});

	return (
		<Fragment>
			<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(350px, 3fr))'>
				{
					cryptoAccountCryptocurrencies.map((cryptoAccountCryptocurrency: ClientCryptoAccountCryptocurrencyEntity) => 
						<CryptoAccountCryptocurrency onReloadCryptoAccountCryptocurrencies={reloadCryptoAccountCryptocurrencies} 
							cryptoAccountCryptocurrency={cryptoAccountCryptocurrency}
							key={cryptoAccountCryptocurrency.id}/>)
				}
			</SimpleGrid>
		</Fragment>
	);
};

export default CryptoAccountCryptocurrenciesList;