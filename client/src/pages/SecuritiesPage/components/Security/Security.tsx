import { Button, Card, Flex, Icon, Link, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useEffect, useState } from 'react';
import { HiOutlineBuildingOffice2 } from 'react-icons/hi2';
import { getIconUrl } from '../../../../api/securities/securityApi';
import { SecurityEntity } from '../../../../models/securities/SecurityEntity';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import StoredIcon from '../../../../shared/components/StoredIcon';

type Props = {
    security: SecurityEntity,
    onEditClicked: (security: SecurityEntity) => void,
    onDeleteClicked: (security: SecurityEntity) => void
}

const Security = (props: Props) => {
    const {id, name, ticker, type, actualPrice, currency, iconKey} = props.security;

    const securityLink = `../security/${id}`;

    const iconUrl = iconKey ? getIconUrl(iconKey) : undefined;

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Stack justifyContent={"start"} direction="row" alignItems="center">
                            <StoredIcon
                                src={iconUrl}
                                fallbackIcon={<HiOutlineBuildingOffice2 size={20} color="#aaa" />}
                                size="md"
                            />
                            <Link fontSize="2xl" fontWeight={600} color="text_primary" href={securityLink}>{ticker}</Link>
                        </Stack>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={600}>{type.name}</Text>
                        <Text fontWeight={600}>{formatMoneyByCurrencyCulture(actualPrice, currency.name)}</Text>
                    </Stack>
                    <Flex gap={1}>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onEditClicked(props.security)}>
                            <Icon color="card_action_icon_primary">
                                <MdEdit/>
                            </Icon>
                        </Button>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onDeleteClicked(props.security)}>
                            <Icon color="card_action_icon_danger">
                                <MdDelete/>
                            </Icon>
                        </Button>
                    </Flex>
                </Flex>
            </Card.Body>
        </Card.Root>
    </Fragment>
};

export default Security;