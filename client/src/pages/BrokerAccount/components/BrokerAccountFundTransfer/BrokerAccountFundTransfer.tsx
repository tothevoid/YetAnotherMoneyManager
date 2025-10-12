import { Card, Flex, Span, Stack, Text, CardBody, Button, Icon } from '@chakra-ui/react';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { formatDate } from '../../../../shared/utilities/formatters/dateFormatter';
import { useTranslation } from 'react-i18next';
import { BrokerAccountFundTransferEntity } from '../../../../models/brokers/BrokerAccountFundTransfer';
import { MdDelete } from 'react-icons/md';

interface Props {
    fundTransfer: BrokerAccountFundTransferEntity,
    onDeleteClicked: (fundTransfer: BrokerAccountFundTransferEntity) => void
}

const BrokerAccountFundTransfer = (props: Props) => {
    const { account, amount, date, income} = props.fundTransfer;

    const sign = income ? "+" : "-";

    const { i18n } = useTranslation();

    return <Card.Root borderColor="border_primary" color="text_primary" backgroundColor="background_primary" 
        mt={5} mb={5} boxShadow={"sm"} _hover={{ boxShadow: "md" }}>
        <CardBody>
            <Flex justifyContent="space-between" alignItems="center">
                <Stack direction={'row'} alignItems="center">
                    <Text textAlign={'center'} w={150} rounded={10} padding={1} background={'purple.600'}>{formatDate(date, i18n, true)}</Text>
                    <Text fontWeight={700}>{props.fundTransfer.account?.name}</Text>
                </Stack>
                <Flex gap={2} justifyContent="space-between" alignItems="center">
                    <Text>
                        <Span>{sign} {formatMoneyByCurrencyCulture(amount, account?.currency.name)} </Span>
                    </Text>
                    <Button background={'background_secondary'} size={'sm'} onClick={() => props.onDeleteClicked(props.fundTransfer)}>
                        <Icon color="card_action_icon_danger">
                            <MdDelete/>
                        </Icon>
                    </Button>
                </Flex>
            </Flex>
        </CardBody>
    </Card.Root>
};

export default BrokerAccountFundTransfer;