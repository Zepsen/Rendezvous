<template>
    <div>       
        <v-list subheader>
            <v-subheader>Rooms</v-subheader>
            <v-list-tile
              v-for="room in rooms"
              :key="room.name"
              avatar
              @click="showRoom(room.name)">              
  
              <v-list-tile-content>
                <v-list-tile-title v-html="room.name"></v-list-tile-title>
              </v-list-tile-content>
  
              <v-list-tile-action>
                <v-icon :color="room.active ? 'teal' : 'grey'">chat_bubble</v-icon>
              </v-list-tile-action>
            </v-list-tile>
        </v-list>
        <AddRoom /> <!-- Imported AddRoom component -->
   </div>

    
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { EventBus } from './Event';
import AddRoom from './AddRoom.vue';

@Component({
    components: {
        AddRoom,
    }
})
export default class Rooms extends Vue {
    private rooms: object[] =  [
           {id: 1, name: 'PHP Room'},
           {id: 2, name: 'Python Room'},
           {id: 3, name: 'Daily standup'}
       ];

    private roomCount: number = 3; // used to keep track of the number of rooms present
    private loading: boolean = false; // indicate when tracks in a room is being loaded
    
    private showRoom(room: any) {
        EventBus.$emit('show_room', room);
    }

    created() {
        EventBus.$on('new_room', this.onAddRoom);
    }

    private onAddRoom(roomName: string) {
        debugger
        this.roomCount++;
        this.rooms.push({id: this.roomCount, name: roomName});
    }
}
</script>
