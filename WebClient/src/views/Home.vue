<!-- Template -->

<template>  
    <div>
        <v-btn @click="$router.push({path: '/notification'})">To notification</v-btn>
        <v-btn @click="$router.push({path: '/conference'})">To conference</v-btn>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class Home extends Vue {}
</script>
