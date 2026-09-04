import { Badge, Box, Button, Card, Flex, HStack, Stack, Text } from '@chakra-ui/react';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { FaBitcoin } from 'react-icons/fa';
import { MdDelete, MdEdit } from 'react-icons/md';
import { getIconUrl } from '../../../../api/crypto/cryptocurrencyApi';
import { CryptoAccountCryptocurrencyEntity } from '../../../../models/crypto/CryptoAccountCryptocurrencyEntity';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import StoredIcon from '../../../../shared/components/StoredIcon';

type Props = {
    cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity;
    onReloadCryptoAccountCryptocurrencies: () => void;
    onEditClicked: (cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity) => void;
    onDeleteClicked: (cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity) => void;
};

const CryptoAccountCryptocurrency: React.FC<Props> = ({
    cryptoAccountCryptocurrency,
    onEditClicked,
    onDeleteClicked,
}) => {
    const { quantity, cryptocurrency } = cryptoAccountCryptocurrency;
    const { t } = useTranslation();

    const iconUrl = cryptocurrency.iconKey ? getIconUrl(cryptocurrency.iconKey) : undefined;
    const totalValue = quantity * cryptocurrency.price;

    return (
        <Card.Root
            backgroundColor="background_primary"
            borderColor="border_primary"
            overflow="hidden"
            position="relative"
            borderRadius="xl"
            transition="transform 0.2s, box-shadow 0.2s, border-color 0.2s"
            _hover={{ transform: "translateY(-2px)", boxShadow: "lg", borderColor: "teal.500" }}
        >
            <Box h="3px" bg="teal.500" w="full" />

            <Card.Body color="text_primary" p={4.5}>
                <Stack spaceY={3.5}>
                    <Flex justify="space-between" align="center">
                        <Flex align="center" gap={3}>
                            <StoredIcon
                                src={iconUrl}
                                fallbackIcon={<FaBitcoin size={22} color="#aaa" />}
                                size="md"
                            />
                            <Stack direction={"row"} spaceY={0}>
                                <Text fontSize="lg" fontWeight="900" color="text_primary">
                                    {cryptocurrency.name}
                                </Text>
                                <Flex gap={1.5} align="center">
                                    <Badge size="xs" variant="solid" colorPalette="teal">
                                        {cryptocurrency.symbol}
                                    </Badge>
                                </Flex>
                            </Stack>
                        </Flex>

                        <HStack gap={1}>
                            <Button
                                size="xs"
                                variant="subtle"
                                color="gray.400"
                                bg="transparent"
                                _hover={{ color: "card_action_icon_primary", bg: "background_secondary" }}
                                onClick={() => onEditClicked(cryptoAccountCryptocurrency)}
                            >
                                <MdEdit size={16} />
                            </Button>
                            <Button
                                size="xs"
                                variant="subtle"
                                color="gray.400"
                                bg="transparent"
                                _hover={{ color: "card_action_icon_danger", bg: "background_secondary" }}
                                onClick={() => onDeleteClicked(cryptoAccountCryptocurrency)}
                            >
                                <MdDelete size={16} />
                            </Button>
                        </HStack>
                    </Flex>

                    <Flex justify="space-between" align="baseline" pt={1}>
                        <Stack spaceY={0}>
                            <Text fontSize="xs" color="gray.400" fontWeight="medium">
                                {t("crypto_account_cryptocurrency_quantity")}
                            </Text>
                            <Text fontSize="md" fontWeight="bold">
                                {quantity} {cryptocurrency.symbol}
                            </Text>
                            <Text fontSize="2xs" color="gray.400">
                                1 {cryptocurrency.symbol} = {formatMoneyByCurrencyCulture(cryptocurrency.price, "USD")}
                            </Text>
                        </Stack>

                        <Stack spaceY={0} align="flex-end">
                            <Text fontSize="xs" color="gray.400" fontWeight="medium">
                                {t("crypto_account_cryptocurrency_total_value")}
                            </Text>
                            <Text fontSize="2xl" fontWeight="900" letterSpacing="tight">
                                {formatMoneyByCurrencyCulture(totalValue, "USD")}
                            </Text>
                        </Stack>
                    </Flex>
                </Stack>
            </Card.Body>
        </Card.Root>
    );
};

export default CryptoAccountCryptocurrency;