<script setup lang="ts">
import EmailInput from '@/components/forms/inputs/EmailInput.vue';
import PasswordInput from '@/components/forms/inputs/PasswordInput.vue';
import LogoBanner from '@/components/LogoBanner.vue'
import { key } from '@/stores/auth';
import { login } from '@/utils/api/auth/login';
import { ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useStore } from 'vuex';

const store = useStore(key)
const router = useRouter()
const route = useRoute()


const email = ref('')
const password = ref('')

const isSuccess = ref<boolean>()
const loading = ref(false)
const regSuccess = route.query.reg == 'success'

const handleLogin = async () => {
    loading.value = true
    const result = await login(email.value, password.value)
    isSuccess.value = result.isSuccess

    if (result.isSuccess) {
        store.commit("storeLogin", result)
        router.push({ name: 'Calendar', query: { status: regSuccess ? 'new' : undefined } })
    }

    loading.value = false
}

</script>

<template>
    <v-container class="pt-10">

        <logo-banner class="mb-5" />

        <v-card class="mx-auto pa-12 pt-9 pb-8" elevation="8" max-width="448" rounded="lg">
            <v-alert v-if="regSuccess" text="Your registration was successful, now you can log in."
                title="Successful registration" type="success" variant="tonal" class="mb-4"></v-alert>

            <div class="text-center text-h5">Account</div>

            <EmailInput v-model="email" :disabled="loading" />

            <PasswordInput v-model="password" :disabled="loading" />

            <v-card v-if="isSuccess === false" class="mb-12 mt-2" color="error" variant="tonal">
                <v-card-text class="text-medium-emphasis text-caption">
                    Something went wrong, make sure you entered the correct email and password.
                </v-card-text>
            </v-card>

            <v-btn class="mb-8" color="blue" size="large" variant="tonal" block v-on:click="handleLogin"
                :loading="loading">
                Log In
            </v-btn>

            <v-card-text class="text-center">
                <router-link class="text-blue text-decoration-none" to="/register">
                    Sign up now <v-icon icon="mdi-chevron-right"></v-icon>
                </router-link>
            </v-card-text>
        </v-card>
    </v-container>
</template>
