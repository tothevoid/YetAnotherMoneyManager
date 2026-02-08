import { MdDelete, MdEdit, MdContentCopy } from "react-icons/md";
import { Card, Flex, Stack, Button, Text, Container, Icon, Image, Progress } from "@chakra-ui/react";
import {  formatNumericDate } from "../../../../shared/utilities/formatters/dateFormatter";
import { formatMoney } from "../../../../shared/utilities/formatters/moneyFormatter";
import { DepositEntity } from "../../../../models/deposits/DepositEntity";
import { useTranslation } from "react-i18next";
import { getBankIconUrl } from "../../../../api/banks/bankApi";

interface Props {
    deposit: DepositEntity
    onEditClicked: (deposit: DepositEntity) => void,
    onCloneClicked: (deposit: DepositEntity) => void,
    onDeleteClicked: (deposit: DepositEntity) => void
}

const Deposit: React.FC<Props> = ({deposit, onEditClicked, onCloneClicked, onDeleteClicked}) => {
    const { i18n, t } = useTranslation();

    const estimatedEarnTitle = deposit.to > new Date() ?
        t("entity_deposit_estimated_earn"):
        t("entity_deposit_earned");

    const daysPassed = Math.floor((new Date().getTime() - new Date(deposit.from).getTime()) / (1000 * 3600 * 24));
    const totalDays = Math.floor((new Date(deposit.to).getTime() - new Date(deposit.from).getTime()) / (1000 * 3600 * 24));
    const progressValue = totalDays > 0 ? (daysPassed / totalDays) * 100 : 0;

    return <Card.Root borderColor="border_primary" color="text_primary" backgroundColor="background_primary">
        <Card.Body boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
            <Stack>
                <Flex gapX={2} alignItems={"center"} justifyContent={"left"} direction={"row"}>
                    {deposit?.bank?.iconKey && <Image fit={"contain"} h={6} w={6} rounded={4} src={getBankIconUrl(deposit?.bank?.iconKey)}/>}
                    <Text fontSize={"xl"} fontWeight={600}>
                        {deposit.name}
                    </Text>
                </Flex>

                <Progress.Root colorPalette={"green"} defaultValue={progressValue}>
                    <Progress.Track >
                        <Progress.Range />
                    </Progress.Track>
                </Progress.Root>
               
                <Container padding={0}>
                    <Flex justifyContent="space-between">
                        <Text color={"gray.500"}>{t("entity_deposit_initial_amount")}:</Text>
                        <Text>{formatMoney(deposit.initialAmount)}</Text>
                    </Flex>
                    <Flex justifyContent="space-between">
                        <Text color={"gray.500"}>{t("entity_deposit_percentage")}:</Text>
                        <Text >{deposit.percentage}%</Text>
                    </Flex>
                    <Flex justifyContent="space-between">
                        <Text color={"gray.500"}>{t("entity_deposit_dates")}:</Text>
                        <Text color={"green.500"}>{`${formatNumericDate(deposit.from, i18n)} - ${formatNumericDate(deposit.to, i18n)}`}</Text>
                    </Flex>
                    <Flex justifyContent="space-between">
                        <Text color={"gray.500"}>{estimatedEarnTitle}:</Text>
                        <Text color={"green.500"}>+{formatMoney(deposit?.estimatedEarn ?? 0)}</Text>
                    </Flex>
                    <Flex gap={2} paddingTop={4} justifyContent="end">
                        <Button background={'background_secondary'} onClick={() => onEditClicked(deposit)} size={'sm'}>
                            <Icon color="card_action_icon_primary">
                                <MdEdit/>
                            </Icon>
                        </Button>
                        <Button background={'background_secondary'} onClick={() => onCloneClicked(deposit)} size={'sm'}>
                            <Icon color="card_action_icon_primary">
                                <MdContentCopy/>
                            </Icon>
                        </Button>
                        <Button background={'background_secondary'} onClick={() => onDeleteClicked(deposit)} size={'sm'}>
                            <Icon color="card_action_icon_danger">
                                <MdDelete/>
                            </Icon>
                        </Button>
                    </Flex>
                </Container>
            </Stack>
        </Card.Body>
    </Card.Root>
}

export default Deposit;