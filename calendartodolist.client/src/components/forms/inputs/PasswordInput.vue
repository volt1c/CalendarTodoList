<script setup lang="ts">
import { ref } from 'vue';

const password = defineModel({ default: '' })

const visible = ref(false)

defineProps<{
    disabled?: boolean
    confirm?: boolean
}>()
</script>

<template>
    <div class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between mt-3">
        {{ confirm ? 'Confirm' : '' }} Password
    </div>

    <v-text-field :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'" :type="visible ? 'text' : 'password'"
        v-model="password" density="compact" placeholder="Enter your password" prepend-inner-icon="mdi-lock-outline"
        variant="outlined" @click:append-inner="visible = !visible" :disabled="disabled"></v-text-field>
</template>