import { useEffect, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import config from "../../config";

const HANDLER_NAME = "ReceiveServerMessage"

export const useSignalR = (onMessage: (message: string) => Promise<void>) => {
	const connectionRef = useRef<signalR.HubConnection | null>(null);

	useEffect(() => {
		return () => {
			if (connectionRef?.current?.state !== signalR.HubConnectionState.Connected) {
				return;
			}
			
			const connection = connectionRef?.current;
			connection.off(HANDLER_NAME)
			connection.stop();
			connectionRef.current = null;
		}
	}, [])

	useEffect(() => {
		if (connectionRef.current) {
			return;
		}

		const connection = new signalR.HubConnectionBuilder()
			.withUrl(`${config.api.URL}/messages`)
			.withAutomaticReconnect()
			.build();

		connection
			.start()
			.then(() => {
				connection.on(HANDLER_NAME, onMessage);
			})
			.catch(console.error);

		connectionRef.current = connection;
	}, [onMessage]);
};
