import { Button, Card, Flex, Icon, Link, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { BsWallet2 } from 'react-icons/bs';
import { SiBinance } from 'react-icons/si';
import { CryptoAccountEntity } from '../../../../models/crypto/CryptoAccountEntity';
import { getTotalBalance } from '../../../../api/crypto/cryptoAccountCryptocurrencyApi';
import { getCryptoProviderIconUrl } from '../../../../api/crypto/cryptoProviderApi';
import StoredIcon from '../../../../shared/components/StoredIcon/StoredIcon';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';

interface Props {
    cryptoAccount: CryptoAccountEntity;
    onEditClicked: (cryptoAccount: CryptoAccountEntity) => void;
    onDeleteClicked: (cryptoAccount: CryptoAccountEntity) => void;
}

const CryptoAccount = (props: Props) => {
    const { id, name, cryptoProvider } = props.cryptoAccount;
    const { t } = useTranslation();
    const accountLink = `../crypto_account/${id}`;

    const [totalAmount, setTotalAmount] = useState<number>(0);

    const fetchBalance = useCallback(async () => {
        const balance = await getTotalBalance(id);
        setTotalAmount(balance);
    }, [id]);

    useEffect(() => {
        fetchBalance();
    }, [fetchBalance]);

    return (
        <Fragment>
            <Card.Root
                backgroundColor="background_primary"
                borderColor="border_primary"
                borderRadius="xl"
                boxShadow="sm"
                _hover={{ boxShadow: "md" }}
                transition="all 0.2s ease"
            >
                <Card.Body padding={4} color="text_primary">
                    {/* Top row: Account title, provider icon, action buttons */}
                    <Flex justifyContent="space-between" alignItems="flex-start" gap={2} mb={3}>
                        <Stack gap={1} flex={1} minW={0}>
                            <Flex alignItems="center" gap={2} flexWrap="wrap">
                                {cryptoProvider && (
                                    <StoredIcon
                                        src={cryptoProvider.iconKey ? getCryptoProviderIconUrl(cryptoProvider.iconKey) : undefined}
                                        fallbackIcon={<SiBinance size={16} color="#aaa" />}
                                        size="sm"
                                        title={cryptoProvider.name}
                                    />
                                )}
                                <Link
                                    fontSize="xl"
                                    fontWeight={800}
                                    color="text_primary"
                                    href={accountLink}
                                    _hover={{ color: "action_primary" }}
                                >
                                    {name}
                                </Link>
                            </Flex>
                        </Stack>
                        <Flex gap={1} flexShrink={0}>
                            <Button
                                borderColor="background_secondary"
                                background="button_background_secondary"
                                size="sm"
                                onClick={() => props.onEditClicked(props.cryptoAccount)}
                            >
                                <Icon color="card_action_icon_primary">
                                    <MdEdit />
                                </Icon>
                            </Button>
                            <Button
                                borderColor="background_secondary"
                                background="button_background_secondary"
                                size="sm"
                                onClick={() => props.onDeleteClicked(props.cryptoAccount)}
                            >
                                <Icon color="card_action_icon_danger">
                                    <MdDelete />
                                </Icon>
                            </Button>
                        </Flex>
                    </Flex>

                    {/* Balance section */}
                    <Flex alignItems="center" gap={3} p={3} borderRadius="lg" backgroundColor="background_secondary">
                        <Flex
                            w={9}
                            h={9}
                            borderRadius="md"
                            backgroundColor="rgba(234, 179, 8, 0.15)"
                            color="yellow.400"
                            alignItems="center"
                            justifyContent="center"
                            flexShrink={0}
                        >
                            <BsWallet2 size={18} />
                        </Flex>
                        <Stack gap={0}>
                            <Text fontSize="xs" color="text_secondary" fontWeight={500}>
                                {t("crypto_account_card_balance")}
                            </Text>
                            <Text fontSize="xl" fontWeight={800} color="text_primary">
                                {formatMoneyByCurrencyCulture(totalAmount, "USD")}
                            </Text>
                        </Stack>
                    </Flex>
                </Card.Body>
            </Card.Root>
        </Fragment>
    );
};

export default CryptoAccount;