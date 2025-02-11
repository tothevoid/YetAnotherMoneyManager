import './Button.scss'

type Props = {
    onClick:  () => void,
    classes: string,
    text: string
}

const Button = (props: Props) => {
    const classes = `${props.classes} btn-base`;
    return <button className={classes} onClick={()=>props.onClick()}>{props.text}</button>
}

export default Button;