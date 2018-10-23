<!-- Template -->

<template>  
    <v-btn @click="$router.push({path: '/notification'})">To notification</v-btn>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class Home extends Vue {}
</script>
