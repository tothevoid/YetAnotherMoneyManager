import { Checkbox } from "@chakra-ui/react"

export interface SwitchButtonProps {
    title: string,
    active: boolean,
    onSwitch: (active: boolean) => void
}

const SwitchButton: React.FC<SwitchButtonProps> = ({title, active, onSwitch}) => {
    return <Checkbox.Root checked={active} 
        onCheckedChange={(details) => onSwitch(!!details.checked)} variant="solid">
        <Checkbox.HiddenInput />
        <Checkbox.Control />
        <Checkbox.Label color="text_primary">{title}</Checkbox.Label>
    </Checkbox.Root>
}

export default SwitchButton;