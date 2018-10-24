<template>
    <v-app>
        <base-header/>
        <base-nav/>
        <v-content>
            <v-container fluid>                
                <router-view/>
            </v-container>
        </v-content>
        <base-footer v-if="isPublicPage"/>
    </v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Getter, namespace } from 'vuex-class';

import BaseHeader from '@/components/layouts/BaseHeader.vue';
import BaseNav from '@/components/layouts/BaseNav.vue';
import BaseFooter from '@/components/layouts/BaseFooter.vue';

const LayoutGetter = namespace('layout', Getter);

@Component({
    components: {
        BaseFooter, BaseHeader, BaseNav
    },
})
export default class App extends Vue {
    @LayoutGetter isPublicPage: boolean;
}
</script>

<style lang="less">

    #localTrack {        
        video {
            width: 250px;
            height: 250px;
        }
    }
</style>
