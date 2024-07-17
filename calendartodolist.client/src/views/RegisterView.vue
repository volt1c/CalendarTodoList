<script setup lang="ts">
import LogoBanner from '@/components/LogoBanner.vue'
import { register, type ErrorInfo } from '@/utils/api/auth/register';
import { emailRules } from '@/utils/rules/emilRules';
import { ref, watch } from 'vue';

const visible = ref(false)
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
        // redirect
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
            <div class="text-subtitle-1 text-medium-emphasis">Account</div>

            <v-text-field density="compact" placeholder="Email address" prepend-inner-icon="mdi-email-outline"
                variant="outlined" :rules="emailRules" v-model="email"></v-text-field>

            <div class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between mt-3">
                Password

                <a class="text-caption text-decoration-none text-blue" href="#" rel="noopener noreferrer"
                    target="_blank">
                    Forgot login password?
                </a>
            </div>

            <v-text-field :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'" :type="visible ? 'text' : 'password'"
                v-model="password" density="compact" placeholder="Enter your password"
                prepend-inner-icon="mdi-lock-outline" variant="outlined"
                @click:append-inner="visible = !visible"></v-text-field>

            <div class="text-subtitle-1 text-medium-emphasis">Confirm Password</div>

            <v-text-field :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'" :type="visible ? 'text' : 'password'"
                v-model="passwordRepeat" density="compact" placeholder="Enter your password"
                prepend-inner-icon="mdi-lock-outline" variant="outlined"
                @click:append-inner="visible = !visible"></v-text-field>

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
