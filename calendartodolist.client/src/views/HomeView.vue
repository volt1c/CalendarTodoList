<script setup lang="ts">
</script>

<template>
    <div class="w-100 h-100 bg-waves">
        <v-container class="h-100 justify-center d-flex flex-column drop-shadow">
            <v-row class="justify-center my-10">
                <v-col cols="12" class="text-center justify-center">
                    <v-img src="/logo.png" max-width="128" class="my-4 ml-auto mr-auto"></v-img>
                    <h1 class="text-h2 my-4">Welcome to Calendar</h1>
                    <p class="subtitle-1">Your ultimate planner application</p>
                </v-col>
            </v-row>
        </v-container>
    </div>
</template>

<style scoped>
.bg-waves {
    background-image: url('/waves.svg') !important;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center bottom;
}

.drop-shadow {
    filter: drop-shadow(4px 4px 4px #004a8f9a) drop-shadow(4px 4px 8px #ffffff79) drop-shadow(4px 4px 16px #ffffffa8) drop-shadow(4px 4px 32px #ffffffbd) drop-shadow(4px 4px 64px #ffffffc7);
}
</style>