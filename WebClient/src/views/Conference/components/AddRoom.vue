<template>   
       <v-layout column align-center justify-center>                
            <v-text-field type="text" v-model="room_name"></v-text-field>        
            <v-btn @click.prevent="createNewRoom">Add room</v-btn>
        </v-layout>   
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { EventBus } from './Event';

@Component
export default class AddRoom extends Vue {
    private room_name: string  = "";
    private authenticated: boolean  = false;

    private createNewRoom() {
        if(!this.room_name) {
            alert("please provide a room name");
            return
        }
       
       EventBus.$emit('new_room', this.room_name);
       this.room_name = '';
    }
}

</script>


