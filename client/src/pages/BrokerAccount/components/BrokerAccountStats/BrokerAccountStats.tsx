import { Fragment, useEffect, useState } from "react";
import { getBrokerAccountStats } from "../../../../api/brokers/brokerAccountSummaryApi";
import { BrokerAccountSummaryEntity } from "../../../../models/brokers/BrokerAccountSummaryEntity";
import { SimpleGrid } from "@chakra-ui/react";
import { useUserProfile } from "../../../../../features/UserProfileSettingsModal/hooks/UserProfileContext";
import { BrokerAccountEntity } from "../../../../models/brokers/BrokerAccountEntity";
import MoneyCard from "../../../../shared/components/MoneyCard/MoneyCard";
import BrokerAccountTransfersHistoryChart from "../BrokerAccountTransfersHistoryChart/BrokerAccountTransfersHistoryChart";

interface Props {
    brokerAccount: BrokerAccountEntity
}

const BrokerAccountStats: React.FC<Props> = ({ brokerAccount }) => {    
    const { user } = useUserProfile();

    const [stats, setStats] = useState<BrokerAccountSummaryEntity | null>(null); 

    useEffect(() => {
        const getData = async () => {
            const result = await getBrokerAccountStats(brokerAccount.id, new Date(), new Date());
            setStats(result);
        }
    
        getData();
    }, [])

    if (!stats || !user) return <Fragment/>;

    const {brokerAccountStats, transferStats} = stats;

    const currencyName = user.currency.name;

    return <SimpleGrid marginBlock={4} gap={4}>
        <SimpleGrid columns={2} gap={4}>
            <MoneyCard title="Инвестировано" value={brokerAccountStats.investedValue} currency={currencyName}/>
            <MoneyCard title="Текущее" value={brokerAccountStats.currentValue} currency={currencyName}/>
        </SimpleGrid>
        <SimpleGrid columns={2} gap={4}>
            <MoneyCard title="Внесено" value={transferStats.totalDeposited} currency={currencyName}/>
            <MoneyCard title="Выведено" value={transferStats.totalWithdrawn} currency={currencyName}/>
        </SimpleGrid>
        <BrokerAccountTransfersHistoryChart brokerAccountId={brokerAccount.id}/>
    </SimpleGrid>
}

export default BrokerAccountStats;