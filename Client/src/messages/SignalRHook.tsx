import {	useEffect, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import config from "../config";

export const useSignalR = (onMessage: (message: string) => Promise<void>) => {
	const connectionRef = useRef<signalR.HubConnection | null>(null);

	useEffect(() => {
		const connection = new signalR.HubConnectionBuilder()
			.withUrl(`${config.api.URL}/messages`)
			.withAutomaticReconnect()
			.build();

		connection
			.start()
			.then(() => {
				connection.on("ReceiveServerMessage", onMessage);
			})
			.catch(console.error);

		connectionRef.current = connection;

		return () => {
			if (connection && connection.state === signalR.HubConnectionState.Connected) {
				connection.stop();
			}
			
		};
	}, [onMessage]);

	return {
		sendMessage: async (message: string) => {
			if (connectionRef.current?.state === signalR.HubConnectionState.Connected) {
				await connectionRef.current.invoke("SendMessage", message);
			}
		},
	};
};
