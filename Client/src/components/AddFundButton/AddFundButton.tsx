import { AddIcon } from "@chakra-ui/icons"
import { Button, FormControl, FormLabel, Input, Modal, ModalBody, ModalCloseButton, ModalContent, 
    ModalFooter, ModalHeader, ModalOverlay, useDisclosure } from "@chakra-ui/react"
import { useState } from "react"

import { Fragment } from "react/jsx-runtime"
import { FundEntity } from "../../models/FundEntity"

type FundProps = {
	onAdded: (fund: FundEntity) => void;
};

const AddFundButton: React.FC<FundProps> = ({ onAdded }) => {
    const { isOpen, onOpen, onClose } = useDisclosure()
    
	const initialState = {
		name: "",
		balance: 0,
	}

    const [formData, setFormData] = useState(initialState);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const { name, value } = e.target;
      setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const onFundAdded = () => {
      onAdded({id: "", name: formData.name, balance: Number(formData.balance)});
	  setFormData(initialState)
      onClose();
    };

    // const initialRef: React.RefObject<FocusableElement> = React.useRef(null)
    // const finalRef: React.RefObject<FocusableElement> = React.useRef(null)

    return (
      <Fragment>
         <Button onClick={onOpen} leftIcon={<AddIcon/>} colorScheme='purple' size='md'>
            Add fund
        </Button>
        <Modal
        //   initialFocusRef={initialRef}
        //   finalFocusRef={finalRef}
          isOpen={isOpen}
          onClose={onClose}
        >
          <ModalOverlay />
          <ModalContent>
            <ModalHeader>New fund</ModalHeader>
            <ModalCloseButton />
            <ModalBody pb={6}>
              <FormControl>
                <FormLabel>Name</FormLabel>
                {/* ref={initialRef} */}
                <Input name="name" value={formData.name} onChange={handleChange} placeholder='Debit card' />
              </FormControl>
  
              <FormControl mt={4}>
                <FormLabel>Balance</FormLabel>
                <Input type="number" name="balance" value={formData.balance} onChange={handleChange} placeholder='10000' />
              </FormControl>

              {/* <FormControl mt={4}>
                <FormLabel>Currency</FormLabel>
                <Input placeholder='10000' />
              </FormControl> */}
            </ModalBody>
            <ModalFooter>
              <Button onClick={onFundAdded} colorScheme='blue' mr={3}>Add</Button>
              <Button onClick={onClose} >Cancel</Button>
            </ModalFooter>
          </ModalContent>
        </Modal>
      </Fragment>
    )
}

export default AddFundButton