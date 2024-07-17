<script setup lang="ts">
    import LogoImg from '@/components/LogoImg.vue'
</script>

<template>
    <v-app>
        <v-layout>
            <v-app-bar>
                <v-app-bar-title @click="$router.push('/')">
                    <v-icon start>
                        <logo-img/>
                    </v-icon>Calendar
                </v-app-bar-title>
                <v-spacer></v-spacer>
                <v-btn color="primary"
                       variant="flat"
                       class="mr-2"
                       to="/login">
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
