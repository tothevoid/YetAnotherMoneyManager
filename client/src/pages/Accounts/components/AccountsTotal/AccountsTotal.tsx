import { FaDollarSign, FaEuroSign, FaRubleSign } from "react-icons/fa";
import { Flex, Icon, Text, VStack } from "@chakra-ui/react";
import { AccountCurrencySummary } from "../../../../models/accounts/accountsSummary";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";
import { IconType } from "react-icons/lib";
import { BsCurrencyExchange } from "react-icons/bs";

const getIconByCurrency = (currencyName: string): IconType => {
	switch (currencyName) {
		case "RUB":
			return FaRubleSign
		case "USD":
			return FaDollarSign
		case "EUR":
			return FaEuroSign
	}

	return BsCurrencyExchange;
}

interface CurrencyItemProps {
	icon: IconType;
	value: string;
	code: string;
}

const CurrencyItem: React.FC<CurrencyItemProps> = ({ icon, value, code }) => {
	return (
		<Flex align="center" gap="4">
			<Flex
				w="48px"
				h="48px"
				borderRadius="full"
				align="center"
				justify="center"
				bg="background_secondary"
			>
				<Icon as={icon} color="white" />
			</Flex>

			<VStack align="start">
				<Text color="white" fontSize="lg" fontWeight="semibold">
					{value}
				</Text>
				<Text color="gray.400" fontSize="sm">
					{code}
				</Text>
			</VStack>
		</Flex>
	);
}

interface Props {
	accountCurrencySummaries: AccountCurrencySummary[]
}

const AccountsTotal: React.FC<Props> = ({accountCurrencySummaries}) => {
	if (!accountCurrencySummaries.length) {
		return <></>
	}

	return (
		<Flex
			bg="background_primary"
			p="4"
			marginBlock="4"
			borderRadius="2xl"
			w="fit-content"
			boxShadow="0 0 40px rgba(0,0,0,0.4)"
		> 
			<Flex flex="1"align="center" gapX={10} justify="start">
				{
					accountCurrencySummaries.map((currencySummary) => {
						return (
							<CurrencyItem 
								key={currencySummary.name}
								icon={getIconByCurrency(currencySummary.name)}
								value={formatMoneyByCurrencyCulture(currencySummary.summary, currencySummary.name)}
								code={currencySummary.name}
							/>
						)
					})
				}
			</Flex>
		</Flex>
	);
}

export default AccountsTotal