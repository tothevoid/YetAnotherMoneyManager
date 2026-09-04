import React, { Fragment, useCallback, useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { Card, Flex, SimpleGrid, Text } from "@chakra-ui/react";
import { Nullable } from "../../../../shared/utilities/nullable";
import { CryptoAccountStatsEntity } from "../../../../models/crypto/CryptoAccountStatsEntity";
import { getCryptoAccountStats } from "../../../../api/crypto/cryptoAccountStatsApi";
import DistributionChart from "../../../Dashboard/components/DistributionChart";
import MoneyCard from "../../../../shared/components/MoneyCard/MoneyCard";
import Placeholder from "../../../../shared/components/Placeholder/Placeholder";

interface Props {
    cryptoAccountId?: Nullable<string>;
    dataVersion?: number;
}

const CryptoAccountStats: React.FC<Props> = ({ cryptoAccountId, dataVersion }) => {
    const { t } = useTranslation();
    const [stats, setStats] = useState<CryptoAccountStatsEntity | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    const fetchData = useCallback(async () => {
        setIsLoading(true);
        try {
            const result = await getCryptoAccountStats(cryptoAccountId);
            if (result) {
                setStats(result);
            }
        } finally {
            setIsLoading(false);
        }
    }, [cryptoAccountId]);

    useEffect(() => {
        fetchData();
    }, [fetchData, dataVersion]);

    if (isLoading) {
        return <Fragment />;
    }

    if (!stats || (stats.cryptoDistribution.length === 0 && stats.accountsDistribution.length === 0)) {
        return (
            <Placeholder text={t("crypto_stats_no_data")}>
                <Fragment />
            </Placeholder>
        );
    }

    const hasMultipleAccounts = !cryptoAccountId && stats.accountsDistribution.length > 1;

    return (
        <SimpleGrid marginBlock={4} gap={4}>
            <SimpleGrid columns={{ base: 1, md: stats.mainCurrency !== "USD" ? 2 : 1 }} gap={4}>
                <MoneyCard
                    title={t("crypto_stats_total_value")}
                    value={stats.totalUsd}
                    currency="USD"
                />
                {stats.mainCurrency !== "USD" && (
                    <MoneyCard
                        title={`${t("crypto_stats_total_value")} (${stats.mainCurrency})`}
                        value={stats.totalConverted}
                        currency={stats.mainCurrency}
                    />
                )}
            </SimpleGrid>

            <SimpleGrid columns={{ base: 1, lg: hasMultipleAccounts ? 2 : 1 }} gap={4}>
                <Card.Root backgroundColor="background_primary" borderColor="border_primary" borderRadius="xl">
                    <Card.Body padding={4}>
                        <Flex justifyContent="space-between" alignItems="center" mb={3}>
                            <Text fontSize="md" fontWeight={700} color="text_primary">
                                {t("crypto_stats_cryptocurrencies_distribution")}
                            </Text>
                        </Flex>
                        <DistributionChart data={stats.cryptoDistribution} mainCurrency={stats.mainCurrency} />
                    </Card.Body>
                </Card.Root>

                {hasMultipleAccounts && (
                    <Card.Root backgroundColor="background_primary" borderColor="border_primary" borderRadius="xl">
                        <Card.Body padding={4}>
                            <Flex justifyContent="space-between" alignItems="center" mb={3}>
                                <Text fontSize="md" fontWeight={700} color="text_primary">
                                    {t("crypto_stats_accounts_distribution")}
                                </Text>
                            </Flex>
                            <DistributionChart data={stats.accountsDistribution} mainCurrency={stats.mainCurrency} />
                        </Card.Body>
                    </Card.Root>
                )}
            </SimpleGrid>
        </SimpleGrid>
    );
};

export default CryptoAccountStats;
