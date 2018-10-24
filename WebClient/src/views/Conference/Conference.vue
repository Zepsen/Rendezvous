<template>                    
    <v-layout fill-height column>
        <div>                
            <v-text-field type="text" v-model="username"></v-text-field>        
            <v-btn @click.prevent="submitUsername(username)">Username</v-btn>
        </div>
        <v-layout row v-if="authenticated">
            <Rooms />
            <Video :username="username"/>            
        </v-layout>
        <Logs />
    </v-layout>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import axios from "axios";
import Rooms from './components/Rooms.vue';
import Video from './components/Video.vue';
import Logs from './components/Logs.vue';

@Component({
    components: {
        Rooms,
        Video,
        Logs,        
    }
})
export default class Conference extends Vue {
    private username: string  = "";
    private authenticated: boolean  = false;

    private submitUsername(username: string) {
      if(!username) {
        return alert('please provide a username');
      }

      this.authenticated = true;
   }
}

</script>


