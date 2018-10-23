<template>      
    <div>
        <v-btn @click="$router.push({path: '/'})">To Home</v-btn>    



        <v-layout fill-height row align-center justify-center>
            <h3>HUB</h3>
            <v-text-field type="text" v-model="message"></v-text-field>
            <v-btn @click="send" :loading="disabledBtn">Send</v-btn>
        </v-layout>

        <v-layout fill-height row align-center justify-center>
            <h3>REST</h3>
            <v-text-field type="text" v-model="messageRest"></v-text-field>
            <v-btn @click="sendRest" :loading="disabledBtn">Send</v-btn>
        </v-layout>

        <v-layout fill-height row align-center justify-center>            
            <ul>
                <li v-for="m in messages" :key="m">
                    {{m}}
                </li>
            </ul>
        </v-layout>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

import axios from "axios";

const signalR = require("@aspnet/signalr");
const signalRMsgPack = require("@aspnet/signalr-protocol-msgpack");
let connection: any = null;

@Component
export default class Notification extends Vue {
    private messages: string[] = [];
    
    private message: string = "";
    private messageRest: string = "";

    private disabledBtn: boolean = true;

    async created() {
        connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:44326/hub")
            .withHubProtocol(new signalRMsgPack.MessagePackHubProtocol())
            .build();

        
        connection.on("notificationReceived", (username: string, message: string) => {
            this.messages.push(message);
        });

        await connection.start()
            .then(() => { this.disabledBtn = false; });
    }

    private async send() {
        await connection.send("notification", {Message: this.message})
            .then(() => this.message = "");
    }

    private async sendRest() {
        await axios.post("/api/notifications/message", { Message: this.messageRest})
            .then(() => this.messageRest = '');
        
    }
    
}

</script>


