<script setup lang="ts">
import LogoBanner from '@/components/LogoBanner.vue'
import EmailInput from '@/components/forms/inputs/EmailInput.vue';
import PasswordInput from '@/components/forms/inputs/PasswordInput.vue';
import { register, type ErrorInfo } from '@/utils/api/auth/register';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter()

const loading = ref(false)
const errors = ref<ErrorInfo[]>([])

const email = ref('')
const password = ref('')
const passwordRepeat = ref('')

const handleRegister = async () => {
    loading.value = true

    if (password.value !== passwordRepeat.value) {
        errors.value = [{ id: 'DifferentPasswords', value: "Passwords are not the same." }]
        loading.value = false
        return
    }

    const result = await register(email.value, password.value)
    if (result.isSuccess) {
        loading.value = false
        router.push({ name: 'Login', query: { reg: 'success' } })
        return
    }
    errors.value = result.errors

    loading.value = false
}
</script>

<template>
    <v-container class="pt-10">

        <logo-banner class="mb-5" />

        <v-card class="mx-auto pa-12 pb-8" elevation="8" max-width="448" rounded="lg">
            <div class="text-center text-h5">Account</div>

            <EmailInput v-model="email" :disabled="loading" />

            <PasswordInput v-model="password" :disabled="loading" />

            <PasswordInput confirm v-model="passwordRepeat" :disabled="loading" />

            <v-card v-if="errors.length > 0" color="error" variant="tonal">
                <v-card-text class="text-medium-emphasis text-caption">
                    <v-list>
                        <v-list-item-subtitle v-for="error in errors" :key="error.id">
                            {{ error.value }}
                        </v-list-item-subtitle>
                    </v-list>
                </v-card-text>
            </v-card>

            <v-btn class="mb-8 mt-12" color="blue" size="large" variant="tonal" block v-on:click="handleRegister"
                :loading="loading"> Sign Up </v-btn>

            <v-card-text class="text-center">
                <router-link class="text-blue text-decoration-none" to="/login">
                    Sign in <v-icon icon="mdi-chevron-right"></v-icon>
                </router-link>
            </v-card-text>
        </v-card>
    </v-container>
</template>
