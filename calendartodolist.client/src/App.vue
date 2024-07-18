<script setup lang="ts">
import LogoImg from '@/components/LogoImg.vue'
import { useStore } from 'vuex';
import { key } from './stores/auth';
import { useRouter } from 'vue-router';

const router = useRouter()
const store = useStore(key)

console.log('isAuthorized', store.getters.isAuthorized)

const handleLogout = () => {
    store.commit('storeClear')
    router.push('/')
}
</script>

<template>
    <v-app>
        <v-layout>
            <v-app-bar>
                <v-app-bar-title @click="$router.push('/')" :style="{ minWidth: '120px' }">
                    <v-icon start>
                        <logo-img />
                    </v-icon>Calendar
                </v-app-bar-title>

                <v-spacer class="min-w-0"></v-spacer>

                <v-btn v-if="store.getters.isAuthorized" color="primary" icon="mdi-calendar" variant="text" class="mr-2"
                    to="/calendar"></v-btn>

                <v-btn v-if="store.getters.isAuthorized" color="primary" variant="outlined" class="mr-2"
                    v-on:click="handleLogout">
                    Logout
                    <v-icon icon="mdi-login" end></v-icon>
                </v-btn>
                <v-btn v-else color="primary" variant="flat" class="mr-2" to="/login">
                    Login
                    <v-icon icon="mdi-login" end></v-icon>
                </v-btn>
            </v-app-bar>

            <v-main class="">
                <router-view />
            </v-main>
        </v-layout>
    </v-app>
</template>
