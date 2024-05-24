import { EventEmitter, Injectable } from '@angular/core';
import { ChatMessageModel } from '@app/infrastructure/models/chat-message';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '@env/environment';

@Injectable({
    providedIn: 'root',
})
export class ChatService {
    messageReceived = new EventEmitter<ChatMessageModel>();
    connectionEstablished = new EventEmitter<boolean>();
    private connectionId: string;
    private connectionIsEstablished = false;
    private hubConnection: HubConnection;

    constructor() {
        const authToken = sessionStorage.getItem('authToken');

        if (authToken) {
            this.createConnection(authToken);
            this.registerOnServerEvents();
            this.startConnection();
        }
    }

    public sendMessage(newMessage: ChatMessageModel): void {
        newMessage.fromConnectionId = this.connectionId;
        // eslint-disable-next-line no-console
        this.hubConnection
            .invoke('SendNewMessage', newMessage)
            .catch((error) => console.log(error));
    }

    private createConnection(authToken: string): void {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.hubRoute + '/chathub', {
                accessTokenFactory: () => authToken,
            })
            .build();
    }

    private startConnection(): void {
        this.hubConnection
            .start()
            .then(() => {
                this.connectionIsEstablished = true;
                console.log('Hub connection started');
                return this.getConnectionId();
            })
            .then((connectionId: string) => {
                this.connectionId = connectionId;
                this.connectionEstablished.emit(true);
            })
            .catch((err) => {
                console.log(
                    'Error while establishing connection, retrying...' +
                        JSON.stringify(err),
                );
                setTimeout(() => {
                    this.startConnection();
                }, 5000);
            });
    }

    private getConnectionId() {
        return this.hubConnection.invoke('getConnectionId');
    }

    private registerOnServerEvents(): void {
        this.hubConnection.on('MessageReceived', (data: ChatMessageModel) => {
            this.messageReceived.emit(data);
        });
    }
}
