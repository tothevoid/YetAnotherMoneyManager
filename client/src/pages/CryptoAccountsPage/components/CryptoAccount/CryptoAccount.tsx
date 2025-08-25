import { Button, Card, Flex, Icon, Link, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment } from 'react';
import { CryptoAccountEntity } from '../../../../models/crypto/CryptoAccountEntity';

interface Props {
    cryptoAccount: CryptoAccountEntity
    onEditClicked: (cryptoAccount: CryptoAccountEntity) => void
    onDeleteClicked: (cryptoAccount: CryptoAccountEntity) => void
}

const CryptoAccount = (props: Props) => {
    const {id, name, cryptoProvider} = props.cryptoAccount;
    const accountLink = `../crypto_account/${id}`;

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Link fontSize="2xl" fontWeight={900} color="text_primary" href={accountLink}>{name}</Link>
                        <Text fontWeight={600}>{cryptoProvider.name}</Text>
                    </Stack>
                    <Flex gap={1}>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onEditClicked(props.cryptoAccount)}>
                            <Icon color="card_action_icon_primary">
                                <MdEdit/>
                            </Icon>
                        </Button>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onDeleteClicked(props.cryptoAccount)}>
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

export default CryptoAccount;