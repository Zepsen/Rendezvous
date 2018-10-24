<template>
    <v-layout column align-center class="ma-5 ">
        <div class="log"  v-for="log in logs" v-bind:key="log.id">
            {{log.message}}
        </div>
    </v-layout>
</template>

<script lang="ts">
import { Component, Vue, Emit } from 'vue-property-decorator';
import axios from "axios";
import { EventBus } from './Event'

@Component
export default class Logs extends Vue {
    private logs: object[] =  [];
    private logCount: number = 3;    

    created() {
        EventBus.$on('new_log', (message: string) => {
        this.logs.push( {id: this.logCount, message: message} );

        this.logCount += 1; 
        })   
    }
}

</script>