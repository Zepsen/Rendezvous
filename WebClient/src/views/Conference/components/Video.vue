<template>
   <div>
       <div class="roomTitle">
           <span v-if="loading"> Loading... {{roomName}}</span>
           <span v-else-if="!loading && roomName"> Connected to {{roomName}}</span>
           <span v-else>Select a room to get started</span>
       </div>

       <v-container fluid grid-list-sm>
            <v-layout row wrap>
                <v-flex xs4>
                    <div id="remoteTrack"></div>
                </v-flex>
            </v-layout>
        </v-container>

       <v-spacer></v-spacer>
       
       <div class="wrapperLocalTrack">
           <div id="localTrack"></div>
       </div>
   </div>
</template>

<script lang="ts">
import { Component, Prop, Vue  } from 'vue-property-decorator';
import axios from "axios";
import { EventBus } from './Event';
import Twilio, { connect, createLocalTracks, createLocalVideoTrack } from 'twilio-video';


@Component({
    components: {}
})
export default class Video extends Vue {
    @Prop() username!: string;

    private loading: boolean = false;
    private data: any = {};
    private localTrack: boolean = false;
    private remoteTrack: string = '';
    private activeRoom: any = null;
    private previewTracks: string = '';
    private identity: string = '';
    private roomName: string = '';

    created() {
        EventBus.$on('show_room', (room_name: string) => {
            this.createChat(room_name);
        })

         window.addEventListener('beforeunload', this.leaveRoomIfJoined);
    }

    // Generate access token
    private async getAccessToken() {
        // return await axios.get(`http://localhost:3000/token?identity=${this.username}`);
        return await axios.get("/api/notifications/token", { params: { identity: this.username }});
    };

   // Trigger log events 
   private dispatchLog(message: string) {
       EventBus.$emit('new_log', message);
   };

   // Attach the Tracks to the DOM.
   private attachTracks(tracks: any, container: any) {
       tracks.forEach(function(track: any) {
          container.appendChild(track.attach());
      });
   };

    // Attach the Participant's Tracks to the DOM.
   private attachParticipantTracks(participant: any, container: any) {
       let tracks = Array.from(participant.tracks.values());
        this.attachTracks(tracks, container);
   };

   // Detach the Tracks from the DOM.
   private detachTracks(tracks: any) {
        tracks.forEach( (track: any) => {
          track.detach().forEach((detachedElement: any) => {
             detachedElement.remove();
          });
      });
   };

   // Detach the Participant's Tracks from the DOM.
   private detachParticipantTracks(participant: any) {
        let tracks = Array.from(participant.tracks.values());
        this.detachTracks(tracks);
   };

   // Leave Room.
   private leaveRoomIfJoined() {
       if (this.activeRoom) {
           this.activeRoom.disconnect();
       }
   };

   // Create a new chat
    private createChat(room_name: string) {
        
        this.loading = true;
        const VueThis: any = this;

        this.getAccessToken().then( (data: any) => {
        
          VueThis.roomName = null;          
          const token = data.data.token;
          let connectOptions = {
              name: room_name,
              // logLevel: 'debug',
              audio: true,
              video: true
          };
          // before a user enters a new room,
          // disconnect the user from they joined already
          this.leaveRoomIfJoined();
            
          // remove any remote track when joining a new room
          document.getElementById('remoteTrack').innerHTML = "";

          Twilio.connect(token, connectOptions).then(function(room: any) {
              // console.log('Successfully joined a Room: ', room);
              
              VueThis.dispatchLog('Successfully joined a Room: '+ room_name);

              // set active toom
              VueThis.activeRoom = room;
              VueThis.roomName = room_name;
              VueThis.loading = false;

              // Attach the Tracks of all the remote Participants.
             room.participants.forEach(function(participant: any) {
                  let previewContainer = document.getElementById('remoteTrack');
                  VueThis.attachParticipantTracks(participant, previewContainer);
             });
                
              // When a Participant joins the Room, log the event.
                room.on('participantConnected', function(participant: any) {
                    VueThis.dispatchLog("Joining: '" + participant.identity + "'");
                });
              
              // When a Participant adds a Track, attach it to the DOM.
                room.on('trackAdded', function(track: any, participant: any) {
                   VueThis.dispatchLog(participant.identity + " added track: " + track.kind);
                   let previewContainer = document.getElementById('remoteTrack');
                   VueThis.attachTracks([track], previewContainer);
               });

              // When a Participant removes a Track, detach it from the DOM.
                room.on('trackRemoved', function(track: any, participant: any) {
                   VueThis.dispatchLog(participant.identity + " removed track: " + track.kind);
                   VueThis.detachTracks([track]);
               });

              // When a Participant leaves the Room, detach its Tracks.
                room.on('participantDisconnected', function(participant: any) {
                   VueThis.dispatchLog("Participant '" + participant.identity + "' left the room");
                   VueThis.detachParticipantTracks(participant);
               });

              // if local preview is not active, create it
              if(!VueThis.localTrack) {

                  createLocalVideoTrack().then((track: any) => {
                    let localMediaContainer = document.getElementById('localTrack');
                    localMediaContainer.appendChild(track.attach());
                    VueThis.localTrack = true;
                  });
              }

         });
      })

   };
   
}

</script>

<style lang="less" scoped>
    .wrapperLocalTrack {
        position: fixed;
        bottom: 50px;
        right: 50px;        
    }

    
    
</style>
